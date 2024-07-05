namespace Delivery.Infrastructure.Services.Interfaces.Kafka;

public interface IKafkaConsumerService
{
    Task StartAsync(CancellationToken cancellationToken);
    Task StopAsync(CancellationToken cancellationToken);
}