using Contracts.Messages;
using Delivery.Application.ModelsDto.Orders;
using Delivery.Application.Services.Interfaces.Orders;
using Delivery.Infrastructure.Settings;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Delivery.Infrastructure.Services.Implementations.RabbitMQ;

public class RabbitMqConsumerService(
    ILogger<RabbitMqConsumerService> logger, 
    IServiceProvider serviceProvider,
    IOptions<RabbitMqSettings> settings) 
    : IConsumer<OrderStatusMessage>
{
    public async Task Consume(ConsumeContext<OrderStatusMessage> context)
    {
        if (context.InitiatorId == settings.Value.SystemId)
        {
            logger.LogInformation(
                $"Message with InitiatorId {context.InitiatorId} was sent by this system. Skipping processing.");
            return;
        }
        var order = context.Message;
        using var scope = serviceProvider.CreateScope();
        var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();
        await orderService.UpdateOrderStatusAsync(order.OrderId, order.Status);
        logger.LogInformation("Consumed message: {OrderId}", order.OrderId);
    }
}