using Store.Domain.Entities;

namespace Store.Application.Services.Interfaces.Kafka;

public interface IKafkaProducer
{
    Task OrderCreatedAsync(Order order);
}