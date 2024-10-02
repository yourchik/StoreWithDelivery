using Contracts.Messages;

namespace Delivery.Infrastructure.Services.Interfaces.Kafka;

public interface IKafkaProducerService
{
    Task OrderStatusUpdateAsync(OrderStatusMessage statusMessage);
}