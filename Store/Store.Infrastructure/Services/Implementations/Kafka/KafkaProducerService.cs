using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Store.Domain.Entities;
using Store.Infrastructure.Services.Interfaces.Kafka;
using Store.Infrastructure.Settings;

namespace Store.Infrastructure.Services.Implementations.Kafka;

public class KafkaProducerService : IKafkaProducerService
{
    private readonly IProducer<Null, string> _producer;
    private readonly ILogger<KafkaProducerService> _logger;
    private readonly KafkaSettings _kafkaSettings;

    public KafkaProducerService(IOptions<KafkaSettings> kafkaSettings, ILogger<KafkaProducerService> logger)
    {
        _kafkaSettings = kafkaSettings.Value;
        var config = new ProducerConfig
        {
            BootstrapServers = _kafkaSettings.BootstrapServers,
            ClientId = _kafkaSettings.ClientId
        };
        _producer = new ProducerBuilder<Null, string>(config).Build();
        _logger = logger;
    }

    public async Task OrderCreatedAsync(Order order)
    {
        try
        {
            var message = JsonSerializer.Serialize(order);
            var result = await _producer.ProduceAsync(_kafkaSettings.TopicProduce, new Message<Null, string> { Value = message });
            _logger.LogInformation(
                $"Produced message to topic {result.Topic}, partition {result.Partition}, offset {result.Offset}");
        }
        catch (ProduceException<Null, string> ex)
        {
            _logger.LogError($"Failed to produce message: {ex.Error.Reason}");
            throw;
        }
    }
}