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

    public async Task<IEnumerable<Order>> GetOrders() => 
        await _orderRepository.GetAll();
    
    public async Task<Order?> GetOrder(int id) =>
        await _orderRepository.GetById(id);

    public async Task CreateOrder(Order order) =>
        await _orderRepository.Add(order);

    public async Task UpdateOrderStatus(int orderId, Status status) => 
        await _orderRepository.UpdateStatus(orderId, status);
    
    public async Task CancelOrder(int id) =>
        await _orderRepository.Delete(id);
    
}