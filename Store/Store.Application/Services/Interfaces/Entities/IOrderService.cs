using Store.Application.Dtos.OrderDtos;
using Store.Application.Services.Implementations.Results;
using Store.Application.Services.Interfaces.Results;
using Store.Domain.Entities;
using Store.Domain.Repositories.Utilities;

namespace Store.Application.Services.Interfaces.Entities;

public interface IOrderService
{
    Task<EntityResult<IEnumerable<Order>>> GetOrdersByFilterAsync(BaseFilter<Order> filter, int page, int pageSize);
    Task<EntityResult<Order>> GetOrderAsync(Guid id);
    Task<EntityResult<Order>> CreateOrderAsync(CreateOrderDto order);
    Task<IResult> UpdateOrderStatusAsync(Guid orderId, OrderStatus status);
    Task<IResult> CancelOrderAsync(Guid id);
    Task<EntityResult<OrderStatus>> GetOrderStatusAsync(Guid id);
}