using Contracts.Messages;
using Store.Application.Services.Interfaces.Integration;
using Store.Infrastructure.Services.Implementations.RabbitMQ;

namespace Store.Infrastructure.Services.Implementations.Integration;

public class DeliveryService(RabbitMqProducerService rabbitMqProducerService) : IDeliveryService
{
    public async Task SendOrderToDeliveryAsync(OrderCreateMessage order)
    { 
        await rabbitMqProducerService.OrderCreatedAsync(order);
    }

    public async Task SendCancelOrderToDeliveryAsync(OrderStatusMessage orderStatusMessage)
    {
        await rabbitMqProducerService.OrderCreatedAsync(orderStatusMessage);
    }
}
