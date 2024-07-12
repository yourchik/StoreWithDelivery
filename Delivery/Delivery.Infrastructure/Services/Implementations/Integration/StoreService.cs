using Delivery.Application.ModelsDto;
using Delivery.Application.Services.Interfaces.Integration;
using Delivery.Infrastructure.Services.Interfaces.Kafka;
using Delivery.Infrastructure.Services.Interfaces.Sheduler;

namespace Delivery.Infrastructure.Services.Implementations.Integration;

public class StoreService : IStoreService
{
    private readonly IKafkaProducerService _kafkaProducerService;

    public StoreService(IKafkaProducerService kafkaProducerService, IHangFireService hangFireService)
    {
        _kafkaProducerService = kafkaProducerService;
    }

    public async Task SendUpdateStatusOrderAsync(OrderStatusMessage order)
    { 
        await _kafkaProducerService.OrderStatusUpdateAsync(order);
    }
}