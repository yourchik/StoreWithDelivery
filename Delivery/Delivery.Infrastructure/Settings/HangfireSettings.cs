namespace Delivery.Infrastructure.Settings;

public class HangfireSettings
{
    public string StatusUpdateInterval { get; set; }
    
    public string HangfireConnection { get; set; }
    
    public TimeSpan GetStatusUpdateInterval()
    {
        return TimeSpan.Parse(StatusUpdateInterval);
    }
}