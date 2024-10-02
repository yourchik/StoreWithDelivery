namespace Delivery.Infrastructure.Settings;

public class KafkaSettings
{
    public string BootstrapServers { get; init; }
    public string ClientId { get; init; }
    public string GroupId { get; init; }
    public string TopicConsume { get; init; }
    public string TopicProduce { get; init; }
}