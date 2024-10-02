using Contracts.Enum;
using Store.Domain.Entities;

namespace Store.Domain.Repositories.Interfaces;

public interface IOrderRepository : IBaseRepository<Order>
{
    Task<(bool IsSuccess, string ErrorMessage)> UpdateStatusAsync(Order order, OrderStatus status);
}