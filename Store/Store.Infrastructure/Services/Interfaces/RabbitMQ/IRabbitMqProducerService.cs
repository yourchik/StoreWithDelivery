using Store.Domain.Entities;

namespace Store.Infrastructure.Services.Interfaces.RabbitMQ;

public interface IRabbitMqProducerService
{
    Task OrderCreatedAsync(Order order);
}