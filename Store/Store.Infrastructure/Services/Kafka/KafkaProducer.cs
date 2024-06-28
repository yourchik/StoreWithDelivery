using Store.Application.Services.Interfaces.Kafka;
using Store.Domain.Entities;

namespace Store.Infrastructure.Services.Kafka;

public class KafkaProducer : IKafkaProducer
{
    public Task OrderCreatedAsync(Order order)
    {
        throw new NotImplementedException();
    }
}