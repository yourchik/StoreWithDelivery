using Store.Domain.Entities;

namespace Store.Domain.Interfaces;

public interface IOrderRepository : IBaseRepository<Order>
{
    Task UpdateStatusAsync(Guid id, OrderStatus status);
}