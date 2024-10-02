using System.Linq.Expressions;
using Delivery.Infrastructure.Services.Interfaces.Scheduler;
using Hangfire;

namespace Delivery.Infrastructure.Services.Implementations.Scheduler;

public class HangFireService(IRecurringJobManager recurringJobManager)
    : IHangFireService
{
    public void Execute<TService>(Expression<Action<TService>> expr, string cronExpression)
    {
        recurringJobManager.AddOrUpdate(typeof(TService).Name, expr, cronExpression);
    }
}