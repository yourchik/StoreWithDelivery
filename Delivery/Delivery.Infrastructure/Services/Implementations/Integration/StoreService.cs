using Delivery.Application.ModelsDto;
using Delivery.Application.Services.Interfaces.Integration;
using Delivery.Infrastructure.Services.Interfaces.RabbitMQ;

namespace Delivery.Infrastructure.Services.Implementations.Integration;

public class StoreService : IStoreService
{
    private readonly IRabbitMqProducerService _rabbitMqProducer;

    public StoreService(IRabbitMqProducerService kafkaProducerService)
    {
        _rabbitMqProducer = kafkaProducerService;
    }

    public async Task SendUpdateStatusOrderAsync(OrderStatusMessage order)
    { 
        await _rabbitMqProducer.OrderStatusUpdateAsync(order);
    }
}