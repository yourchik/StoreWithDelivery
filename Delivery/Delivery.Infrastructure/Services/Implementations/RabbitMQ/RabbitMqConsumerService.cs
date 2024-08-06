using System.Text;
using System.Text.Json;
using Delivery.Application.ModelsDto;
using Delivery.Infrastructure.Services.Implementations.Sheduler.Jobs;
using Delivery.Infrastructure.Services.Interfaces.RabbitMQ;
using Delivery.Infrastructure.Services.Interfaces.Sheduler;
using Delivery.Infrastructure.Settings;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Delivery.Infrastructure.Services.Implementations.RabbitMQ;

public class RabbitMqConsumerService : BackgroundService, IRabbitMqConsumerService
{
    private readonly RabbitMqSettings _rabbitMqSettings;
    private readonly HangfireSettings _hangfireSettings;
    private readonly ILogger<RabbitMqConsumerService> _logger;
    private readonly IHangFireService _hangFireService;
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public RabbitMqConsumerService(IOptions<RabbitMqSettings> rabbitMqSettings, 
        IOptions<HangfireSettings> hangfireSettings, 
        ILogger<RabbitMqConsumerService> logger, 
        IHangFireService hangFireService)
    {
        _rabbitMqSettings = rabbitMqSettings.Value;
        _logger = logger;
        _hangFireService = hangFireService;
        _hangfireSettings = hangfireSettings.Value;

        var factory = new ConnectionFactory()
        {
            HostName = _rabbitMqSettings.HostName,
            Port = _rabbitMqSettings.Port,
            UserName = _rabbitMqSettings.UserName,
            Password = _rabbitMqSettings.Password
        };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(queue: _rabbitMqSettings.QueueConsume, durable: true, exclusive: false, autoDelete: false, arguments: null);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var order = JsonSerializer.Deserialize<Order>(message);
            _logger.LogInformation("Consumed message: {message}", message);
            if (order == null)
                return;
            _hangFireService.Execute<UpdateOrderStatusJob>(e => e.RunAsync(order, stoppingToken), _hangfireSettings.GetStatusUpdateInterval());
        };

        _channel.BasicConsume(queue: _rabbitMqSettings.QueueConsume, autoAck: true, consumer: consumer);
        return Task.CompletedTask;
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await base.StopAsync(cancellationToken);
        _channel.Close();
        _connection.Close();
    }
}