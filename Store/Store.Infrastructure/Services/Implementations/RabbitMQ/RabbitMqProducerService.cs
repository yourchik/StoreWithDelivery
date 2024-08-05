using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using Store.Domain.Entities;
using Store.Infrastructure.Services.Interfaces.RabbitMQ;
using Store.Infrastructure.Settings;

namespace Store.Infrastructure.Services.Implementations.RabbitMQ;

public class RabbitMqProducerService : IRabbitMqProducerService
{
    private readonly ILogger<RabbitMqProducerService> _logger;
    private readonly RabbitMqSettings _rabbitMqSettings;
    private readonly IModel _channel;

    public RabbitMqProducerService(IOptions<RabbitMqSettings> rabbitMqSettings, ILogger<RabbitMqProducerService> logger)
    {
        _rabbitMqSettings = rabbitMqSettings.Value;
        _logger = logger;

        var factory = new ConnectionFactory()
        {
            HostName = _rabbitMqSettings.HostName,
            Port = _rabbitMqSettings.Port,
            UserName = _rabbitMqSettings.UserName,
            Password = _rabbitMqSettings.Password
        };
        var connection = factory.CreateConnection();
        _channel = connection.CreateModel();
        _channel.QueueDeclare(queue: _rabbitMqSettings.QueueProduce, durable: true, exclusive: false, autoDelete: false, arguments: null);
    }

    public Task OrderCreatedAsync(Order order)
    {
        var message = JsonSerializer.Serialize(order);
        var body = Encoding.UTF8.GetBytes(message);

        _channel.BasicPublish(exchange: "", routingKey: _rabbitMqSettings.QueueProduce, basicProperties: null, body: body);
        _logger.LogInformation($"Produced message to queue {_rabbitMqSettings.QueueProduce}");

        return Task.CompletedTask;
    }
}