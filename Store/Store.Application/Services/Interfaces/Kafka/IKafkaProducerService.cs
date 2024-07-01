using Store.Domain.Entities;

namespace Store.Application.Services.Interfaces.Kafka;

public interface IKafkaProducerService
{
    Task OrderCreatedAsync(Order order);
}