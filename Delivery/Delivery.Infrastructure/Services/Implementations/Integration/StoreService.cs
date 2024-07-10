using Delivery.Application.ModelsDto;
using Delivery.Application.Services.Interfaces.Integration;
using Delivery.Infrastructure.Services.Interfaces.Kafka;

namespace Delivery.Infrastructure.Services.Implementations.Integration;

public class StoreService : IStoreService
{
    private readonly IKafkaProducerService _kafkaProducerService;

    public StoreService(IKafkaProducerService kafkaProducerService)
    {
        _kafkaProducerService = kafkaProducerService;
    }

    public async Task SendUpdateStatusOrderAsync(OrderStatusMessage order)
    { 
        await _kafkaProducerService.OrderStatusUpdateAsync(order);
    }
}