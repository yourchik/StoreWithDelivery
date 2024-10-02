

using System.Text;
using System.Text.Json;
using Contracts.Enum;
using Delivery.Application.ModelsDto.Orders;
using Delivery.Domain.Entities;
using Delivery.Infrastructure.Services.Interfaces.RabbitMQ;
using Delivery.Infrastructure.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Delivery.Infrastructure.Services.Implementations.RabbitMQ;

public class RabbitMqMessageService(
    IOptions<RabbitMqSettings> rabbitMqSettings, 
    ILogger<RabbitMqMessageService> logger) 
    : IRabbitMqMessageService
{
    public async Task<IEnumerable<OrderDto>> GetMessagesAsync(CancellationToken cancellationToken)
    {
        var factory = new ConnectionFactory
        {
            HostName = rabbitMqSettings.Value.HostName,
            Port = rabbitMqSettings.Value.Port,
            UserName = rabbitMqSettings.Value.UserName,
            Password = rabbitMqSettings.Value.Password
        };
        
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        var messages = new List<OrderDto>();
        while (true)
        {
            var result = channel.BasicGet(rabbitMqSettings.Value.QueueOrderCreate, autoAck: false);
            if (result == null)
            {
                logger.LogInformation("No more messages to process.");
                break;
            }
            try
            {
                var body = result.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                using var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(message));
                var orderMessage =
                    await JsonSerializer.DeserializeAsync<OrderDto>(memoryStream, cancellationToken: cancellationToken);
                if (orderMessage == null)
                {
                    logger.LogWarning("Received a null or invalid message.");
                    channel.BasicNack(result.DeliveryTag, multiple: false, requeue: false);
                    continue;
                }

                orderMessage.Status = OrderStatus.Accepted;
                messages.Add(orderMessage);
                logger.LogInformation($"Processed order: {orderMessage.Id}");
                channel.BasicAck(result.DeliveryTag, multiple: false);
            }
            catch (JsonException ex)
            {
                logger.LogError(ex, "Error processing message, message will not be acknowledged.");
                channel.BasicNack(result.DeliveryTag, multiple: false, requeue: false);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error processing message, message will not be acknowledged.");
                channel.BasicNack(result.DeliveryTag, multiple: false, requeue: true);
            }
        }
        return messages;
    }
}