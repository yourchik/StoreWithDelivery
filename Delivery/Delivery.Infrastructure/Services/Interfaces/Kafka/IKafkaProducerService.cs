using Delivery.Application.ModelsDto;

namespace Delivery.Infrastructure.Services.Interfaces.Kafka;

public interface IKafkaProducerService
{
    Task OrderStatusUpdateAsync(OrderStatusMessage message);
}