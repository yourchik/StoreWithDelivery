using System.Transactions;
using Contracts.Enum;
using Contracts.Messages;
using Store.Application.ModelsDto.Orders;
using Store.Application.Services.Factories;
using Store.Application.Services.Implementations.Results;
using Store.Application.Services.Interfaces.AuditManagement;
using Store.Application.Services.Interfaces.Entities;
using Store.Application.Services.Interfaces.Integration;
using Store.Application.Services.Interfaces.Results;
using Store.Domain.Entities;
using Store.Domain.Repositories.Interfaces;
using Store.Domain.Repositories.Utilities;

namespace Store.Application.Services.Implementations.Entities;

public class OrderService(
    IOrderRepository orderRepository,
    IProductService productService,
    IUserService userService,
    IDeliveryService deliveryService,
    IAuditService auditService)
    : IOrderService
{
    public async Task<EntityResult<IEnumerable<Order>>> GetOrdersByFilterAsync(BaseFilter<Order> filter, int page,
        int pageSize)
    {
        var (orders, isSuccess, errorMessage) = await orderRepository.GetByFilterAsync(filter, page, pageSize);
        if (!isSuccess)
            return EntityResult<IEnumerable<Order>>.Failure(errorMessage);

        return EntityResult<IEnumerable<Order>>.Success(orders);
    }

    public async Task<EntityResult<Order>> GetOrderAsync(Guid id)
    {
        var (order, isSuccess, errorMessage) = await orderRepository.GetByIdAsync(id);
        if (!isSuccess && !string.IsNullOrEmpty(errorMessage))
            return EntityResult<Order>.Failure(errorMessage);

        return EntityResult<Order>.Success(order);
    }

    public async Task<EntityResult<Order>> CreateOrderAsync(CreateOrderDto orderDto)
    {
        using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        var productResults = await orderDto.Products
            .ToAsyncEnumerable()
            .SelectAwait(async product => await productService.GetProductAsync(product.Id))
            .ToListAsync();

        var errors = productResults
            .Where(result => !result.IsSuccess)
            .SelectMany(result => result.Errors)
            .ToList();

        if (errors.Count != 0)
            return EntityResult<Order>.Failure(errors.ToArray());

        var products = productResults
            .Where(result => result is { IsSuccess: true, Value: not null })
            .Select(result => result.Value!)
            .ToList();

        var productDictionary = products.ToDictionary(p => p.Id);
        foreach (var productInOrder in orderDto.Products)
        {
            if (!productDictionary.TryGetValue(productInOrder.Id,
                    out var productFromDb)
                || productFromDb.Amount < productInOrder.Amount)
                return EntityResult<Order>.Failure($"""
                                                    Недостаточное количество для продукта с ID: {productInOrder.Id}. 
                                                    Запрашиваемое количество: {productInOrder.Amount}, 
                                                    доступное количество: {productFromDb?.Amount ?? 0}.
                                                    """);

            var reductionAmount = -productInOrder.Amount;
            await productService.UpdateAmountAsync(productInOrder.Id, reductionAmount);
        }

        var user = await userService.GetCurrentUserAsync();
        var order = new Order
        {
            Id = Guid.NewGuid(),
            Products = products,
            Address = orderDto.Address,
            User = user
        };

        var (isSuccess, errorMessage) = await orderRepository.CreateAsync(order);
        if (!isSuccess)
            return EntityResult<Order>.Failure(errorMessage);

        await auditService.AuditChange(order.GetType().FullName!, order.Id, user.Id);
        await deliveryService.SendOrderToDeliveryAsync(new OrderCreateMessage(order.Id, order.Address, order.Status));
        transaction.Complete();
        return EntityResult<Order>.Success(order);
    }

    public async Task<IResult> UpdateOrderStatusAsync(Guid orderId, OrderStatus status)
    {
        var order = await orderRepository.GetByIdAsync(orderId);
        if (!order.IsSuccess)
            return ResultFactory.CreateResult(order.IsSuccess, order.ErrorMessage);

        await auditService.AuditChange(order.Entity.GetType().FullName!, order.Entity.Id, order.Entity.UserId,
            nameof(order.Entity.Status), order.Entity.Status.ToString(), status.ToString());
        
        var (isSuccess, errorMessage) = await orderRepository.UpdateStatusAsync(order.Entity, status);
        if (!isSuccess)
            return ResultFactory.CreateResult(isSuccess, errorMessage);
        
        return ResultFactory.CreateResult(isSuccess);
    }

    public async Task<IResult> CancelOrderAsync(Guid id)
    {
        var order = await orderRepository.GetByIdAsync(id);
        if (!order.IsSuccess)
            return ResultFactory.CreateResult(order.IsSuccess, order.ErrorMessage);
        
        await auditService.AuditChange(order.Entity.GetType().FullName!, order.Entity.Id, order.Entity.UserId,
            nameof(order.Entity.Status), order.Entity.Status.ToString(), OrderStatus.Cancelled.ToString());
        
        var (isSuccess, errorMessage) = await orderRepository.UpdateStatusAsync(order.Entity, OrderStatus.Cancelled);
        if (!isSuccess)
            return ResultFactory.CreateResult(isSuccess, errorMessage);

        await deliveryService.SendCancelOrderToDeliveryAsync(new OrderStatusMessage(order.Entity.Id,
            order.Entity.Status));

        return ResultFactory.CreateResult(isSuccess);
    }

    public async Task<EntityResult<OrderStatus>> GetOrderStatusAsync(Guid id)
    {
        var (order, isSuccess, errorMessage) = await orderRepository.GetByIdAsync(id);

        if (!isSuccess && !string.IsNullOrEmpty(errorMessage))
            return EntityResult<OrderStatus>.Failure(errorMessage);

        return EntityResult<OrderStatus>.Success(order.Status);
    }
}