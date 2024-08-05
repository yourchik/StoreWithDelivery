using Delivery.Application.ModelsDto;

namespace Delivery.Infrastructure.Services.Interfaces.RabbitMQ;

public interface IRabbitMqProducerService
{
    Task OrderStatusUpdateAsync(OrderStatusMessage orderStatusMessage);
}