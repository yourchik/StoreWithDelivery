using System.Text.Json;
using Confluent.Kafka;
using Contracts.Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Store.Application.Services.Interfaces.Entities;
using Store.Infrastructure.Services.Interfaces.Kafka;
using Store.Infrastructure.Settings;

namespace Store.Infrastructure.Services.Implementations.Kafka;

public class KafkaConsumerService : BackgroundService, IKafkaConsumerService
{
    private readonly KafkaSettings _kafkaSettings;
    private readonly ILogger<KafkaConsumerService> _logger;
    private readonly IConsumer<Null, string> _consumer;
    private readonly IServiceProvider _serviceProvider;

    public KafkaConsumerService(IOptions<KafkaSettings> kafkaSettings, ILogger<KafkaConsumerService> logger, IServiceProvider serviceProvider)
    {
        _kafkaSettings = kafkaSettings.Value;
        _logger = logger;
        _serviceProvider = serviceProvider;

        var config = new ConsumerConfig
        {
            BootstrapServers = _kafkaSettings.BootstrapServers,
            GroupId = _kafkaSettings.GroupId,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        _consumer = new ConsumerBuilder<Null, string>(config).Build();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _consumer.Subscribe(_kafkaSettings.TopicConsume);
        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var consumeResult = _consumer.Consume(stoppingToken);
                var message = consumeResult.Message.Value;
                var orderStatus = JsonSerializer.Deserialize<OrderStatusMessage>(message);
                if (orderStatus == null)
                    continue;
                _logger.LogInformation(
                    $"Received order status update: OrderId = {orderStatus.OrderId}, Status = {orderStatus.Status}");
                using var scope = _serviceProvider.CreateScope();
                var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();
                await orderService.UpdateOrderStatusAsync(orderStatus.OrderId, orderStatus.Status);
            }
        }
        catch (OperationCanceledException)
        {
            _consumer.Close();
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await base.StopAsync(cancellationToken);
        _consumer.Close();
    }
}