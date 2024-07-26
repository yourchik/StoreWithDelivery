using Store.Domain.Entities;

namespace Store.Domain.Interfaces;

public interface IOrderRepository : IBaseRepository<Order>
{
    Task<(bool IsSuccess, string ErrorMessage)> UpdateStatusAsync(Guid id, OrderStatus status);
}