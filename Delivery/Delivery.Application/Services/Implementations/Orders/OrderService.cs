using Contracts.Enum;
using Contracts.Messages;
using Delivery.Application.ModelsDto.Orders;
using Delivery.Application.Services.Interfaces.Integration;
using Delivery.Application.Services.Interfaces.Orders;
using Delivery.Domain.Entities;
using Delivery.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Delivery.Application.Services.Implementations.Orders;

public class OrderService(IStoreService storeService, ILogger<OrderService> logger, IOrderRepository orderRepository) : IOrderService
{
    public async Task CreateRangeOrdersAsync(IEnumerable<OrderDto> ordersDto)
    {
        var orders = ordersDto
            .Select(orderDto => new Order { 
                Id = orderDto.Id,
                Address = orderDto.Address, 
                Status = OrderStatus.Accepted})
            .ToList();
        logger.LogInformation($"Orders: {orders} created");
        await orderRepository.CreateRangeAsync(orders);
    }

    public async  Task UpdateOrderStatusAsync(Guid id, OrderStatus updateStatus)
    {
        var order = await orderRepository.GetOrderAsync(id);
        await orderRepository.UpdateStatusAsync(order, updateStatus);
        logger.LogInformation($"Order status {updateStatus} was updated");
        var orderStatusMessage = new OrderStatusMessage(id, updateStatus);
        await storeService.SendUpdateStatusOrderAsync(orderStatusMessage);
    }
    
    public async Task CloseOrderAsync(Guid id)
    {
        var order = await orderRepository.GetOrderAsync(id);
        if (order.Status == OrderStatus.Delivered)
            throw new Exception();

        await UpdateOrderStatusAsync(id, OrderStatus.Cancelled);
    }

    public async Task NextOrderAsync(Guid id)
    {
        var order = await orderRepository.GetOrderAsync(id);
        var orderStatusUpdate = order.Status + 1;
        if (orderStatusUpdate == OrderStatus.Delivered)
            throw new Exception();

        await UpdateOrderStatusAsync(id, orderStatusUpdate);
    }
}