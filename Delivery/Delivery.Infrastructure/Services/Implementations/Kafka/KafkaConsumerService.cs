using Delivery.Infrastructure.Services.Interfaces.Kafka;

namespace Delivery.Infrastructure.Services.Implementations.Kafka;

public class KafkaConsumerService : IKafkaConsumerService
{
    public Task ConsumeAsync(CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}