using Contracts.Messages;
using Delivery.Application.ModelsDto.Orders;

namespace Delivery.Infrastructure.Services.Interfaces.Kafka;

public interface IKafkaProducerService
{
    Task OrderStatusUpdateAsync(OrderStatusMessage statusMessage);
}