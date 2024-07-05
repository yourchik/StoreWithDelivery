using Delivery.Application.ModelsDto;
using Delivery.Application.Services.Interfaces.Integration;
using Delivery.Application.Services.Interfaces.Orders;
using Microsoft.Extensions.Logging;

namespace Delivery.Application.Services.Implementations.Orders;

public class OrderService : IOrderService
{
    private readonly IStoreService _storeService;
    private readonly ILogger<OrderService> _logger;
    private readonly TimeSpan _statusUpdateInterval = TimeSpan.FromSeconds(5);
    private static readonly OrderStatus[] _statuses = { 
        OrderStatus.Created, 
        OrderStatus.Accepted, 
        OrderStatus.Sent, 
        OrderStatus.Delivered, 
        OrderStatus.Received, 
        OrderStatus.Cancelled 
    };


    public OrderService(IStoreService storeService, ILogger<OrderService> logger)
    {
        _storeService = storeService;
        _logger = logger;
    }

    public async  Task UpdateOrderStatus(Order order)
    {
        foreach (var status in _statuses)
        {
            if (order.Status == OrderStatus.Cancelled)
            {
                _logger.LogInformation("Order {orderId} was cancelled", order.Id);
                return;
            }
            await Task.Delay(_statusUpdateInterval);
            order.Status = status;
            _logger.LogInformation("Order status {orderStatus} was updated", order.Status);
            var orderStatusMessage = new OrderStatusMessage {OrderId = order.Id, Status = order.Status};
            await _storeService.SendUpdateStatusOrderAsync(orderStatusMessage);
        }
    }
}