using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Store.Application.Dtos.Order;
using Store.Domain.Interfaces;
using Store.Infrastructure.Services.Interfaces.RabbitMQ;
using Store.Infrastructure.Settings;

namespace Store.Infrastructure.Services.Implementations.RabbitMQ;

public class RabbitMqConsumerService : BackgroundService, IRabbitMqConsumerService
{
    private readonly RabbitMqSettings _rabbitMqSettings;
    private readonly ILogger<RabbitMqConsumerService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public RabbitMqConsumerService(IOptions<RabbitMqSettings> rabbitMqSettings, ILogger<RabbitMqConsumerService> logger, IServiceProvider serviceProvider)
    {
        _rabbitMqSettings = rabbitMqSettings.Value;
        _logger = logger;
        _serviceProvider = serviceProvider;

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
            var orderStatus = JsonSerializer.Deserialize<OrderStatusMessage>(message);
            if (orderStatus == null)
                return;

            _logger.LogInformation(
                $"Received order status update: OrderId = {orderStatus.OrderId}, Status = {orderStatus.Status}");

            using var scope = _serviceProvider.CreateScope();
            var orderService = scope.ServiceProvider.GetRequiredService<IOrderRepository>();
            await orderService.UpdateStatusAsync(orderStatus.OrderId, orderStatus.Status);
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