using Contracts.Messages;

namespace Store.Application.Services.Interfaces.Integration;

public interface IDeliveryService
{
    Task SendOrderToDeliveryAsync(OrderCreateMessage order);
    Task SendCancelOrderToDeliveryAsync(OrderStatusMessage orderStatusMessage);
}