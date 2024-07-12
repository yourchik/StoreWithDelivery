using Delivery.Application.ModelsDto;

namespace Delivery.Infrastructure.Services.Interfaces.Sheduler;

public interface IJob<TItem> where TItem : class
{
    Task RunAsync(TItem item, CancellationToken ct = default);
}
