using System.Text.Json;
using Confluent.Kafka;
using Delivery.Application.ModelsDto;
using Delivery.Application.Services.Interfaces.Orders;
using Delivery.Infrastructure.Services.Interfaces.Kafka;
using Delivery.Infrastructure.Settings;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Delivery.Infrastructure.Services.Implementations.Kafka;

public class KafkaConsumerService : BackgroundService, IKafkaConsumerService
{
    private readonly IOptions<KafkaSettings> _configuration;
    private readonly ILogger<KafkaConsumerService> _logger;
    private readonly IConsumer<Null, string> _consumer;
    private readonly IOrderService _orderService;

    public KafkaConsumerService(IOptions<KafkaSettings> configuration, ILogger<KafkaConsumerService> logger, IOrderService orderService)
    {
        var config = new ConsumerConfig
        {
            GroupId = configuration.Value.GroupId,
            BootstrapServers = configuration.Value.BootstrapServers,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };
        
        _configuration = configuration;
        _logger = logger;
        _consumer = new ConsumerBuilder<Null, string>(config).Build();
        _orderService = orderService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            _consumer.Subscribe(_configuration.Value.TopicConsume);
            while (!stoppingToken.IsCancellationRequested)
            {
                var consumeResult = _consumer.Consume(stoppingToken);
                var message = consumeResult.Message.Value;
                var order = JsonSerializer.Deserialize<Order>(message);
                _logger.LogInformation("Consumed message: {message}", message);
                if (order != null) 
                    await _orderService.UpdateOrderStatus(order);
            }
        }
        catch (OperationCanceledException)
        {
            _consumer.Close();
            _logger.LogInformation("The consumer has been closed");
        }
    }
    
    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await base.StopAsync(cancellationToken);
        _consumer.Close();
    }
}