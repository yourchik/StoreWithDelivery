namespace Delivery.Infrastructure.Services.Interfaces.Kafka;

public interface IKafkaConsumerService
{
    Task ConsumeAsync(CancellationToken ct);
}