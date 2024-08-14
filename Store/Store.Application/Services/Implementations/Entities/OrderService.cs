using Store.Application.Dtos.Order;
using Store.Application.Services.Factories;
using Store.Application.Services.Implementations.Results;
using Store.Application.Services.Interfaces.Entities;
using Store.Application.Services.Interfaces.Integration;
using Store.Application.Services.Interfaces.Results;
using Store.Domain.Entities;
using Store.Domain.Interfaces;

namespace Store.Application.Services.Implementations.Entities;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductService _productService;
    private readonly IDeliveryService _deliveryService;

    public OrderService(IOrderRepository orderRepository, 
        IProductService productService,
        IDeliveryService deliveryService
        )
    {
        _orderRepository = orderRepository;
        _productService = productService;
        _deliveryService = deliveryService;
    }

    public async Task<EntityResult<IEnumerable<Order>>> GetOrdersAsync()
    {
        var (orders, isSuccess, errorMessage) = await _orderRepository.GetAllAsync();
        if (!isSuccess)
            return EntityResult<IEnumerable<Order>>.Failure(errorMessage);

        return EntityResult<IEnumerable<Order>>.Success(orders);
    }
    
    public async Task<EntityResult<Order>> GetOrderAsync(Guid id)
    {
        var (order, isSuccess, errorMessage) = await _orderRepository.GetByIdAsync(id);
    
        if (!isSuccess && !string.IsNullOrEmpty(errorMessage))
            return EntityResult<Order>.Failure(errorMessage);
        
        return order != null ? EntityResult<Order>.Success(order) : EntityResult<Order>.Failure("Order not found.");
    }

    public async Task<EntityResult<Order>> CreateOrderAsync(CreateOrderDto orderDto)
    {
        var productResults = (await orderDto.ProductsGuid
            .ToAsyncEnumerable()
            .SelectAwait(async productGuid => await _productService.GetProductAsync(productGuid))
            .ToListAsync());
        
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
        
        var order = new Order
        {
            Id = Guid.NewGuid(),
            Products = products,
            Address = orderDto.Address
        };

        await _orderRepository.AddAsync(order);
        await _deliveryService.SendOrderToDeliveryAsync(order);
        return EntityResult<Order>.Success(order);
    }

    public async Task<IResult> UpdateOrderStatusAsync(Guid orderId, OrderStatus status)
    {
        var (isSuccess, errorMessage) = await _orderRepository.UpdateStatusAsync(orderId, status);
        if (!isSuccess)
            return ResultFactory.CreateResult(isSuccess, errorMessage);
        return ResultFactory.CreateResult(isSuccess);
    }

    public async Task<IResult> CancelOrderAsync(Guid id)
    {
        var (isSuccess, errorMessage) = await _orderRepository.UpdateStatusAsync(id, OrderStatus.Cancelled);
        if (!isSuccess)
            return ResultFactory.CreateResult(isSuccess, errorMessage);
        return ResultFactory.CreateResult(isSuccess);
    }
}
