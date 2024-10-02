using Contracts.Enum;
using Delivery.Application.ModelsDto.Orders;
using Delivery.Domain.Entities;

namespace Delivery.Application.Services.Interfaces.Orders;

public interface IOrderService
{
    Task CreateRangeOrdersAsync(IEnumerable<OrderDto> ordersDto);
    Task UpdateOrderStatusAsync(Guid id, OrderStatus newStatus);
    Task CloseOrderAsync(Guid id);
    Task NextOrderAsync(Guid id);
}