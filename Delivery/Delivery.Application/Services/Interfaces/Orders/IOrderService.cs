using Contracts.Enum;
using Contracts.Messages;

namespace Delivery.Application.Services.Interfaces.Orders;

public interface IOrderService
{
    Task CreateRangeOrdersAsync(IEnumerable<OrderCreateMessage> orderCreateMessages);
    Task UpdateOrderStatusAsync(Guid id, OrderStatus newStatus);
    Task CloseOrderAsync(Guid id);
    Task NextOrderAsync(Guid id);
}