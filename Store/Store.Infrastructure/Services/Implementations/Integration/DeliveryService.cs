using Store.Application.Services.Interfaces.Integration;
using Store.Domain.Entities;
using Store.Infrastructure.Services.Interfaces.Kafka;
using Store.Infrastructure.Services.Interfaces.RabbitMQ;

namespace Store.Infrastructure.Services.Implementations.Integration;

public class DeliveryService : IDeliveryService
{
    private IRabbitMqProducerService _rabbitMqProducer;

    public DeliveryService(IRabbitMqProducerService kafkaProducerService)
    {
        _rabbitMqProducer = kafkaProducerService;
    }

    public async Task SendOrderToDeliveryAsync(Order order)
    { 
        await _rabbitMqProducer.OrderCreatedAsync(order);
    }
}
