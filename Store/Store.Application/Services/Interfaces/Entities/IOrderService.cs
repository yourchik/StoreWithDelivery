using Store.Domain.Entities;

namespace Store.Application.Services.Interfaces.Entities;

public interface IOrderService
{
    Task<IEnumerable<Order>> GetOrdersAsync();
    Task<Order?> GetOrderAsync(Guid id);
    Task CreateOrderAsync(Order order);
    Task UpdateOrderStatusAsync(Guid orderId, OrderStatus status);
    Task CancelOrderAsync(Guid id);
}