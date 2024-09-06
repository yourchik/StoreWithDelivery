using Store.Application.ModelsDto.Order;
using Store.Domain.Entities;

namespace Store.Infrastructure.Services.Interfaces.RabbitMQ;

public interface IRabbitMqProducerService
{
    Task OrderCreatedAsync(OrderMessage order);
}