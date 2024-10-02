namespace Delivery.Infrastructure.Services.Interfaces.Scheduler;

public interface IJob
{
    Task RunAsync(CancellationToken ct = default);
}
