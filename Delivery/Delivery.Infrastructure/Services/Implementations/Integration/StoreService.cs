using Contracts.Messages;
using Delivery.Application.Services.Interfaces.Integration;
using Delivery.Infrastructure.Services.Implementations.RabbitMQ;

namespace Delivery.Infrastructure.Services.Implementations.Integration;

public class StoreService(RabbitMqProducerService kafkaProducerService) : IStoreService
{
    public async Task SendUpdateStatusOrderAsync(OrderStatusMessage orderStatus)
    { 
        await kafkaProducerService.OrderStatusUpdateAsync(orderStatus);
    }
}