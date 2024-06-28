using Store.Domain.Entities;

namespace Store.Domain.Interfaces;

public interface IOrderRepository : IBaseRepository<Order>
{
    Task UpdateStatusAsync(int orderId, Status status);
}