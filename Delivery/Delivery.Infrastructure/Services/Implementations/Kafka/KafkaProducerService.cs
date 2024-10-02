using System.Text.Json;
using Confluent.Kafka;
using Contracts.Messages;
using Delivery.Application.ModelsDto.Orders;
using Delivery.Infrastructure.Services.Interfaces.Kafka;
using Delivery.Infrastructure.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Delivery.Infrastructure.Services.Implementations.Kafka;

public class KafkaProducerService : IKafkaProducerService
{
    private readonly IProducer<Null, string> _producer;
    private readonly ILogger<KafkaProducerService> _logger;
    private readonly KafkaSettings _configuration;

    public KafkaProducerService(IOptions<KafkaSettings> configuration, ILogger<KafkaProducerService> logger)
    {
        _configuration = configuration.Value;
        var config = new ProducerConfig
        {
            BootstrapServers = configuration.Value.BootstrapServers,
            ClientId = configuration.Value.ClientId
        };
        _producer = new ProducerBuilder<Null, string>(config).Build();
        _logger = logger;
    }
    
    public async Task OrderStatusUpdateAsync(OrderStatusMessage orderStatusMessage)
    {
        try
        {
            var message = JsonSerializer.Serialize(orderStatusMessage);
            var result = await _producer.ProduceAsync(_configuration.TopicProduce, new Message<Null, string> { Value = message });
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
