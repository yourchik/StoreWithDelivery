using Store.Application.Services.Interfaces.Entities;
using Store.Domain.Entities;
using Store.Domain.Interfaces;

namespace Store.Application.Services.Implementations.Entities;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;

    public OrderService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<IEnumerable<Order>> GetOrdersAsync() => 
        await _orderRepository.GetAllAsync();
    
    public async Task<Order?> GetOrderAsync(Guid id) =>
        await _orderRepository.GetByIdAsync(id);

    public async Task CreateOrderAsync(Order order) =>
        await _orderRepository.Add(order);

    public async Task UpdateOrderStatusAsync(Guid orderId, OrderStatus status) => 
        await _orderRepository.UpdateStatusAsync(orderId, status);
    
    public async Task CancelOrderAsync(Guid id) =>
        await _orderRepository.DeleteAsync(id);
    
}