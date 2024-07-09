using System.Text.Json;
using Confluent.Kafka;
using Delivery.Application.ModelsDto;
using Delivery.Infrastructure.Services.Interfaces.Kafka;
using Delivery.Infrastructure.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Delivery.Infrastructure.Services.Implementations.Kafka;

public class KafkaProducerService : IKafkaProducerService
{
    private readonly IProducer<Null, string> _producer;
    private readonly ILogger<KafkaProducerService> _logger;
    private readonly IOptions<KafkaSettings> _configuration;

    public KafkaProducerService(IOptions<KafkaSettings> configuration, ILogger<KafkaProducerService> logger)
    {
        var config = new ProducerConfig
        {
            BootstrapServers = configuration.Value.BootstrapServers,
            ClientId = configuration.Value.ClientId
        };
        
        _configuration = configuration;
        _producer = new ProducerBuilder<Null, string>(config).Build();
        _logger = logger;
    }
    
    public async Task ProduceOrderStatusUpdateAsync(OrderStatusMessage orderStatusMessage)
    {
        try
        {
            var message = JsonSerializer.Serialize(orderStatusMessage);
            var result = await _producer.ProduceAsync(_configuration.Value.TopicProduce, new Message<Null, string> { Value = message });
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
