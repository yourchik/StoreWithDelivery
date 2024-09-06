﻿using System.Text.Json;
using Confluent.Kafka;
using Delivery.Application.ModelsDto;
using Delivery.Application.Services.Interfaces.Orders;
using Delivery.Infrastructure.Services.Implementations.Sheduler.Jobs;
using Delivery.Infrastructure.Services.Interfaces.Kafka;
using Delivery.Infrastructure.Services.Interfaces.Sheduler;
using Delivery.Infrastructure.Settings;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Delivery.Infrastructure.Services.Implementations.Kafka;

public class KafkaConsumerService : BackgroundService, IKafkaConsumerService
{
    private readonly KafkaSettings _kafkaSettings;
    private readonly HangfireSettings _hangfireSettings;
    private readonly ILogger<KafkaConsumerService> _logger;
    private readonly IConsumer<Null, string> _consumer;
    private readonly IOrderService _orderService;
    private readonly IHangFireService _hangFireService;

    public KafkaConsumerService(IOptions<KafkaSettings> kafkaSettings, 
        IOptions<HangfireSettings> hangfireSettings, 
        ILogger<KafkaConsumerService> logger, 
        IOrderService orderService, 
        IHangFireService hangFireService)
    {
        _kafkaSettings = kafkaSettings.Value;
        var config = new ConsumerConfig
        {
            GroupId = _kafkaSettings.GroupId,
            BootstrapServers = _kafkaSettings.BootstrapServers,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };
        _logger = logger;
        _consumer = new ConsumerBuilder<Null, string>(config).Build();
        _orderService = orderService;
        _hangFireService = hangFireService;
        _hangfireSettings = hangfireSettings.Value;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            _consumer.Subscribe(_kafkaSettings.TopicConsume);
            while (!stoppingToken.IsCancellationRequested)
            {
                var consumeResult = _consumer.Consume(stoppingToken);
                var message = consumeResult.Message.Value;
                var order = JsonSerializer.Deserialize<OrderMessage>(message);
                _logger.LogInformation("Consumed message: {message}", message);
                if (order == null)
                    continue;
                _hangFireService.Execute<UpdateOrderStatusJob>( e=> e.RunAsync(order, stoppingToken), _hangfireSettings.GetStatusUpdateInterval());
            }
        }
        catch (OperationCanceledException)
        {
            _consumer.Close();
            _logger.LogInformation("The consumer has been closed");
        }

        return Task.CompletedTask;
    }
    
    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await base.StopAsync(cancellationToken);
        _consumer.Close();
    }
}