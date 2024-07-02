using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Store.Domain.Entities;
using Store.Infrastructure.Services.Interfaces.Kafka;

namespace Store.Infrastructure.Services.Implementations.Kafka;

public class KafkaProducerService : IKafkaProducerService
{
    private readonly IProducer<Null, string> _producer;
    private readonly ILogger<KafkaProducerService> _logger;
    private readonly string _topicName;

    public KafkaProducerService(IConfiguration configuration, ILogger<KafkaProducerService> logger)
    {
        var config = new ProducerConfig
        {
            BootstrapServers = configuration["Kafka:BootstrapServers"],
            ClientId = configuration["Kafka:ClientId"]
        };

        _producer = new ProducerBuilder<Null, string>(config).Build();
        _logger = logger;
        _topicName = "order_created_topic";
    }

    public async Task OrderCreatedAsync(Order order)
    {
        var message = JsonSerializer.Serialize(order);
        try
        {
            var result = await _producer.ProduceAsync(_topicName, new Message<Null, string> { Value = message });
            _logger.LogInformation($"Produced message to topic {result.Topic}, partition {result.Partition}, offset {result.Offset}");
        }
        catch (ProduceException<Null, string> ex)
        {
            _logger.LogError($"Failed to produce message: {ex.Error.Reason}");
            throw;
        }
    }
}