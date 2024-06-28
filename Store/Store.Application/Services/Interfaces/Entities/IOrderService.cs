using Store.Domain.Entities;

namespace Store.Application.Services.Interfaces.Entities;

public interface IOrderService
{
    Task<IEnumerable<Order>> GetOrders();
    Task<Order?> GetOrder(int id);
    Task CreateOrder(Order order);
    Task UpdateOrderStatus(int orderId, Status status);
    Task CancelOrder(int id);
}