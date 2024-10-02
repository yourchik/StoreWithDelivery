namespace Store.Infrastructure.Settings;

public class RabbitMqSettings
{
    public string HostName { get; init; }
    public Guid SystemId { get; init; } = Guid.NewGuid();
    public string QueueOrderUpdate { get; init; }
    public string QueueOrderCreate { get; init; }
    public ushort Port { get; init; }
    public string UserName { get; init; }
    public string Password { get; init; }
}