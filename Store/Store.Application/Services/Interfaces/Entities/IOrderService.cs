using Store.Domain.Entities;

namespace Store.Application.Services.Interfaces.Entities;

public interface IOrderService
{
    Task<IEnumerable<Order>> GetOrdersAsync();
    Task<Order?> GetOrderAsync(int id);
    Task CreateOrderAsync(Order order);
    Task UpdateOrderStatusAsync(int orderId, Status status);
    Task CancelOrderAsync(int id);
}