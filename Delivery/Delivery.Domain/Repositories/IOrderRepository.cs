using Contracts.Enum;
using Delivery.Domain.Entities;

namespace Delivery.Domain.Repositories;

public interface IOrderRepository
{
    Task CreateAsync(Order order);
    Task CreateRangeAsync(IEnumerable<Order> orders);
    Task UpdateStatusAsync(Order id, OrderStatus orderStatus);
    Task<Order> GetOrderAsync(Guid id);
}