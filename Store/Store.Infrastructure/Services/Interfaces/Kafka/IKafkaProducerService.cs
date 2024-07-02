using Store.Domain.Entities;

namespace Store.Infrastructure.Services.Interfaces.Kafka;

public interface IKafkaProducerService
{
    Task OrderCreatedAsync(Order order);
}