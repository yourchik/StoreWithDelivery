using Delivery.Application.ModelsDto;
using Delivery.Infrastructure.Services.Interfaces.Kafka;

namespace Delivery.Infrastructure.Services.Implementations.Kafka;

public class KafkaProducerService : IKafkaProducerService
{
    public Task ProduceOrderStatusUpdateAsync(OrderUpdateMessage message)
    {
        throw new NotImplementedException();
    }
}