namespace Store.Infrastructure.Settings;

public class RabbitMqSettings
{
    public string HostName { get; set; }
    public string QueueConsume { get; set; }
    public string QueueProduce { get; set; }
    public int Port { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
}