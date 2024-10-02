using Contracts.Messages;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Store.Application.ModelsDto.Orders;
using Store.Application.Services.Interfaces.Entities;
using Store.Infrastructure.Settings;

namespace Store.Infrastructure.Services.Implementations.RabbitMQ;

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
        var orderStatus = context.Message;
        logger.LogInformation(
            $"Received order status update: OrderId = {orderStatus.OrderId}, Status = {orderStatus.Status}");
        using var scope = serviceProvider.CreateScope();
        var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();
        await orderService.UpdateOrderStatusAsync(orderStatus.OrderId, orderStatus.Status);
    }
}