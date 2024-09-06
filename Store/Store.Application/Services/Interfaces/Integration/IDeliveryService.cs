using Store.Application.ModelsDto.Order;
using Store.Domain.Entities;

namespace Store.Application.Services.Interfaces.Integration;

public interface IDeliveryService
{
    Task SendOrderToDeliveryAsync(OrderMessage order);
}