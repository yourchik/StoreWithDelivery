using Delivery.Application.ModelsDto.Orders;

namespace Delivery.Infrastructure.Services.Interfaces.RabbitMQ;

public interface IRabbitMqMessageService
{ 
    Task<IEnumerable<OrderDto>> GetMessagesAsync(CancellationToken cancellationToke);
}