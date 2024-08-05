using System.Text;
using Delivery.Application.ModelsDto;
using Delivery.Infrastructure.Services.Interfaces.RabbitMQ;
using Delivery.Infrastructure.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace Delivery.Infrastructure.Services.Implementations.RabbitMQ;

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

    public async Task OrderStatusUpdateAsync(OrderStatusMessage orderStatusMessage)
    {
        var message = JsonConvert.SerializeObject(orderStatusMessage);
        var body = Encoding.UTF8.GetBytes(message);

        _channel.BasicPublish(exchange: "", routingKey: _rabbitMqSettings.QueueProduce, basicProperties: null, body: body);
        _logger.LogInformation($"Produced message to queue {_rabbitMqSettings.QueueProduce}");

        await Task.CompletedTask;
    }
}