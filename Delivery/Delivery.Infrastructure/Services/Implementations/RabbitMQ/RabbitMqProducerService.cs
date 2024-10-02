using Contracts.Messages;
using Delivery.Application.ModelsDto.Orders;
using Delivery.Infrastructure.Settings;
using MassTransit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Delivery.Infrastructure.Services.Implementations.RabbitMQ;

public class RabbitMqProducerService(
    IBus bus, 
    ILogger<RabbitMqProducerService> logger,
    IOptions<RabbitMqSettings> settings) 
{
    public async Task OrderStatusUpdateAsync(OrderStatusMessage orderStatusMessage)
    {
        await bus.Publish(orderStatusMessage, context =>
        {
            context.InitiatorId = settings.Value.SystemId;
        });
        logger.LogInformation($"Produced message: {orderStatusMessage}");
    }
    
    public async Task OrderStatusRangeUpdateAsync(ICollection<OrderStatusMessage> orderStatusMessages)
    {
        var tasks = orderStatusMessages.Select(async message =>
        {
            await bus.Publish(message, context =>
            {
                context.InitiatorId = settings.Value.SystemId;
            });
            logger.LogInformation($"Produced message: {message}");
        });
        await Task.WhenAll(tasks);
    }
}