using System.Linq.Expressions;

namespace Delivery.Infrastructure.Services.Interfaces.Scheduler;

public interface IHangFireService
{
    void Execute<TService>(Expression<Action<TService>> expr, string cronExpression);
}