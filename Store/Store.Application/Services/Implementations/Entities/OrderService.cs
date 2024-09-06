using Store.Application.Dtos.OrderDtos;
using Store.Application.Services.Factories;
using Store.Application.Services.Implementations.Results;
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
    IDeliveryService deliveryService)
    : IOrderService
{
    public async Task<EntityResult<IEnumerable<Order>>> GetOrdersByFilterAsync(BaseFilter<Order> filter, int page, int pageSize)
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

        return order != null ? EntityResult<Order>.Success(order) : EntityResult<Order>.Failure("Order not found.");
    }

    public async Task<EntityResult<Order>> CreateOrderAsync(CreateOrderDto orderDto)
    {
        
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
            if (!productDictionary.TryGetValue(productInOrder.Id, out var productFromDb) 
                || productFromDb.Amount < productInOrder.Amount)
            {
                return EntityResult<Order>.Failure($"""
                                                    Недостаточное количество для продукта с ID: {productInOrder.Id}. 
                                                    Запрашиваемое количество: {productInOrder.Amount}, 
                                                    доступное количество: {productFromDb?.Amount ?? 0}.
                                                    """);
            }
            await productService.ReductionAmountUpdate(productInOrder.Id, productInOrder.Amount);
        }
        

        var user = await userService.GetCurrentUserAsync();
        var order = new Order
        {
            Id = Guid.NewGuid(),
            Products = products,
            Address = orderDto.Address,
            User = user
        };
        
        await orderRepository.AddAsync(order);
        await deliveryService.SendOrderToDeliveryAsync(order);
        return EntityResult<Order>.Success(order);
    }

    public async Task<IResult> UpdateOrderStatusAsync(Guid orderId, OrderStatus status)
    {
        var (isSuccess, errorMessage) = await orderRepository.UpdateStatusAsync(orderId, status);
        if (!isSuccess)
            return ResultFactory.CreateResult(isSuccess, errorMessage);
        return ResultFactory.CreateResult(isSuccess);
    }

    public async Task<IResult> CancelOrderAsync(Guid id)
    {
        var (isSuccess, errorMessage) = await orderRepository.UpdateStatusAsync(id, OrderStatus.Cancelled);
        if (!isSuccess)
            return ResultFactory.CreateResult(isSuccess, errorMessage);
        return ResultFactory.CreateResult(isSuccess);
    }

    public async Task<EntityResult<OrderStatus>> GetOrderStatusAsync(Guid id)
    {
        var (order, isSuccess, errorMessage) = await orderRepository.GetByIdAsync(id);

        if (!isSuccess && !string.IsNullOrEmpty(errorMessage))
            return EntityResult<OrderStatus>.Failure(errorMessage);

        return order != null ? EntityResult<OrderStatus>.Success(order.Status) 
            : EntityResult<OrderStatus>.Failure("Order not found.");
    }
}