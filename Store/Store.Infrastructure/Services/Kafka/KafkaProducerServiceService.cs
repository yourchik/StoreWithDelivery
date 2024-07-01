using Store.Application.Services.Interfaces.Kafka;
using Store.Domain.Entities;
namespace Store.Infrastructure.Services.Kafka;

public class KafkaProducerService : IKafkaProducerService
{
    public Task OrderCreatedAsync(Order order)
    {
        throw new NotImplementedException();
    }
}