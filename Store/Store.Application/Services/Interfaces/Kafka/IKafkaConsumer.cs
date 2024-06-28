namespace Store.Application.Services.Interfaces.Kafka;

public interface IKafkaConsumer
{
    Task StartAsync(CancellationToken ct);
    Task StopAsync(CancellationToken ct);  
}