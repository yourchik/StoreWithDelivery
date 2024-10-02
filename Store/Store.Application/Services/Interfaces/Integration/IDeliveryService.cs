using Contracts.Messages;
using Store.Application.ModelsDto.Orders;

namespace Store.Application.Services.Interfaces.Integration;

public interface IDeliveryService
{
    Task SendOrderToDeliveryAsync(OrderMessage order);
    Task SendCancelOrderToDeliveryAsync(OrderStatusMessage orderStatusMessage);
}