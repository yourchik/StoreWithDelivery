namespace Store.Application.Services.Interfaces.Kafka;

public interface IKafkaConsumerService
{
    Task StartAsync(CancellationToken ct);
    Task StopAsync(CancellationToken ct);  
}