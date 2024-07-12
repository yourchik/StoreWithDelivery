using System.Linq.Expressions;
using Delivery.Application.ModelsDto;
using Delivery.Infrastructure.Services.Interfaces.Sheduler;
using Hangfire;
using Hangfire.Common;

namespace Delivery.Infrastructure.Services.Implementations.Sheduler;

public class HangFireService : IHangFireService
{
    private readonly IBackgroundJobClient _backgroundJobClient;
    private readonly IRecurringJobManager _recurringJobManager;

    public HangFireService(IBackgroundJobClient backgroundJobClient, IRecurringJobManager recurringJobManager)
    {
        _backgroundJobClient = backgroundJobClient;
        _recurringJobManager = recurringJobManager;
    }
    public string Execute<TService>(Expression<Action<TService>> expr, TimeSpan delay)
    {
        return _backgroundJobClient.Schedule(expr, delay);
    }
}