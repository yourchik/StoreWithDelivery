using Contracts.Messages;

namespace Delivery.Infrastructure.Services.Interfaces.RabbitMQ;

public interface IRabbitMqMessageService
{ 
    Task<IEnumerable<OrderCreateMessage>> GetMessagesAsync(CancellationToken cancellationToke);
}