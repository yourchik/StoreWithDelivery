using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Store.Application.Dtos;
using Store.Application.Services.Interfaces.Entities;
using Store.Domain.Interfaces;
using Store.Infrastructure.Services.Interfaces.Kafka;

namespace Store.Infrastructure.Services.Implementations.Kafka;

public class KafkaConsumerService : BackgroundService, IKafkaConsumerService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<KafkaConsumerService> _logger;
    private readonly IConsumer<Null, string> _consumer;
    private readonly IOrderRepository _orderService;

    public KafkaConsumerService(IConfiguration configuration, ILogger<KafkaConsumerService> logger, IOrderRepository orderService)
    {
        _configuration = configuration;
        _logger = logger;
        _orderService = orderService;

        var config = new ConsumerConfig
        {
            BootstrapServers = configuration["Kafka:BootstrapServers"],
            GroupId = configuration["Kafka:GroupId"],
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        _consumer = new ConsumerBuilder<Null, string>(config).Build();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _consumer.Subscribe("order_status_topic");

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
                await _orderService.UpdateStatusAsync(orderStatus.OrderId, orderStatus.Status);
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