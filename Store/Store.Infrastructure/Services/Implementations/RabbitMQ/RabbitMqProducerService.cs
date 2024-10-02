using System.Text;
using System.Text.Json;
using Contracts.Messages;
using MassTransit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using Store.Infrastructure.Settings;

namespace Store.Infrastructure.Services.Implementations.RabbitMQ;

public class RabbitMqProducerService
{
    private readonly ILogger<RabbitMqProducerService> _logger;
    private readonly IOptions<RabbitMqSettings> _settings;
    private readonly ConnectionFactory _factory;
    private readonly IBus _bus;

    public RabbitMqProducerService(
        ILogger<RabbitMqProducerService> logger,
        IOptions<RabbitMqSettings> settings,
        IBus bus)
    {
        _logger = logger;
        _settings = settings;
        _bus = bus;
        _factory = new ConnectionFactory
        {
            HostName = _settings.Value.HostName,
            Port = _settings.Value.Port,
            UserName = _settings.Value.UserName,
            Password = _settings.Value.Password
        };
        CreateQueue();
    }

    private void CreateQueue()
    {
        using var connection = _factory.CreateConnection();
        using var channel = connection.CreateModel();
        channel.QueueDeclare(
            queue: _settings.Value.QueueOrderCreate,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        _logger.LogInformation($"Queue {_settings.Value.QueueOrderCreate} created");
    }

    public async Task OrderCreatedAsync(OrderStatusMessage order)
    {
        await _bus.Publish(order);
        _logger.LogInformation($"Produced message for order {order.OrderId} to queue");
    }
    
    public Task OrderCreatedAsync(OrderCreateMessage order)
    {
        using (var connection = _factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            var messageBody = JsonSerializer.Serialize(order);
            var body = Encoding.UTF8.GetBytes(messageBody);

            channel.BasicPublish(
                exchange: "",
                routingKey: _settings.Value.QueueOrderCreate,
                basicProperties: null,
                body: body);

            _logger.LogInformation($"Produced message for order {order.Id} to queue");
        }

        return Task.CompletedTask;
    }
}
