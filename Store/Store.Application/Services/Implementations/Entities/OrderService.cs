using Store.Application.Services.Interfaces.Entities;
using Store.Application.Services.Interfaces.Integration;
using Store.Domain.Entities;
using Store.Domain.Interfaces;

namespace Store.Application.Services.Implementations.Entities;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IDeliveryService _deliveryService;

    public OrderService(IOrderRepository orderRepository, 
        IDeliveryService deliveryService)
    {
        _orderRepository = orderRepository;
        _deliveryService = deliveryService;
    }

    public async Task<IEnumerable<Order>> GetOrdersAsync() => 
        await _orderRepository.GetAllAsync();
    
    public async Task<Order?> GetOrderAsync(Guid id) =>
        await _orderRepository.GetByIdAsync(id);

    public async Task CreateOrderAsync(Order order)
    {
        await _orderRepository.Add(order);
        await _deliveryService.SendOrderToDeliveryAsync(order);
    }
    
    public async Task UpdateOrderStatusAsync(Guid orderId, OrderStatus status) => 
        await _orderRepository.UpdateStatusAsync(orderId, status);
    
    public async Task CancelOrderAsync(Guid id) =>
        await _orderRepository.DeleteAsync(id);
}