using Store.Domain.Entities;

namespace Store.Domain.Interfaces;

public interface IOrderRepository : IBaseRepository<Order>
{
    Task UpdateStatus(int orderId, Status status);
}