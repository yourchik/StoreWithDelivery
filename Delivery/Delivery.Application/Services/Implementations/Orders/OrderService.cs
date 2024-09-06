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
    
    public async  Task UpdateOrderStatus(OrderMessage orderMessage, OrderStatus newStatus)
    {
        if (orderMessage.Status is OrderStatus.Cancelled or OrderStatus.Delivered)
        {
            _logger.LogInformation("Order {orderId} was cancelled or already delivered", orderMessage.Id);
            return;
        }
        
        orderMessage.Status = newStatus;
        _logger.LogInformation("Order status {orderStatus} was updated", orderMessage.Status);
        var orderStatusMessage = new OrderStatusMessage { OrderId = orderMessage.Id, Status = orderMessage.Status };
        await _storeService.SendUpdateStatusOrderAsync(orderStatusMessage);
    }
}