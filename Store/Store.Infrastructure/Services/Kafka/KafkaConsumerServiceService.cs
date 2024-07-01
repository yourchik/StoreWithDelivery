using Store.Application.Services.Interfaces.Kafka;

namespace Store.Infrastructure.Services.Kafka;

public class KafkaConsumerServiceService : IKafkaConsumerService
{
    public Task StartAsync(CancellationToken ct)
    {
        throw new NotImplementedException();
    }   

    public Task StopAsync(CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}