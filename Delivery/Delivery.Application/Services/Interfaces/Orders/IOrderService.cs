using Delivery.Application.ModelsDto;

namespace Delivery.Application.Services.Interfaces.Orders;

public interface IOrderService
{
    Task UpdateOrderStatus(Order order);
}