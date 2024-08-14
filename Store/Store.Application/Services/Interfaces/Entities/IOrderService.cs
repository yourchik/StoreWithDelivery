using Store.Application.Dtos.Order;
using Store.Application.Services.Implementations.Results;
using Store.Application.Services.Interfaces.Results;
using Store.Domain.Entities;

namespace Store.Application.Services.Interfaces.Entities;

public interface IOrderService
{
    Task<EntityResult<IEnumerable<Order>>> GetOrdersAsync();
    Task<EntityResult<Order>> GetOrderAsync(Guid id);
    Task<EntityResult<Order>> CreateOrderAsync(CreateOrderDto order);
    Task<IResult> UpdateOrderStatusAsync(Guid orderId, OrderStatus status);
    Task<IResult> CancelOrderAsync(Guid id);
}