namespace Store.Infrastructure.Settings;

public class KafkaSettings
{
    public string BootstrapServers { get; set; }
    public string ClientId { get; set; }
    public string GroupId { get; set; }
    public string TopicConsume { get; set; }
    public string TopicProduce { get; set; }
}