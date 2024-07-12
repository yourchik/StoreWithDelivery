using Delivery.Application.ModelsDto;
using Delivery.Application.Services.Interfaces.Integration;
using Delivery.Application.Services.Interfaces.Orders;
using Microsoft.Extensions.Logging;

namespace Delivery.Application.Services.Implementations.Orders;

public class OrderService : IOrderService
{
    private readonly IStoreService _storeService;
    private readonly ILogger<OrderService> _logger;

    public OrderService(IStoreService storeService, ILogger<OrderService> logger)
    {
        _storeService = storeService;
        _logger = logger;
    }
    
    public async  Task UpdateOrderStatus(Order order, OrderStatus newStatus)
    {
        if (order.Status == OrderStatus.Cancelled || order.Status == OrderStatus.Delivered)
        {
            _logger.LogInformation("Order {orderId} was cancelled or already delivered", order.Id);
            return;
        }
        
        order.Status = newStatus;
        _logger.LogInformation("Order status {orderStatus} was updated", order.Status);
        var orderStatusMessage = new OrderStatusMessage { OrderId = order.Id, Status = order.Status };
        await _storeService.SendUpdateStatusOrderAsync(orderStatusMessage);
    }
}