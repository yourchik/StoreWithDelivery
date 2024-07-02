using Store.Application.Services.Interfaces.Integration;
using Store.Domain.Entities;
using Store.Infrastructure.Services.Interfaces.Kafka;

namespace Store.Infrastructure.Services.Implementations.Integration;

public class DeliveryService : IDeliveryService
{
    private IKafkaProducerService _kafkaProducerService;

    public DeliveryService(IKafkaProducerService kafkaProducerService)
    {
        _kafkaProducerService = kafkaProducerService;
    }

    public async Task SendOrderToDeliveryAsync(Order order)
    { 
        await _kafkaProducerService.OrderCreatedAsync(order);
    }
}
