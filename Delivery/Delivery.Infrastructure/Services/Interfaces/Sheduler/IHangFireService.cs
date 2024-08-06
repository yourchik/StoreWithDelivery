using System.Linq.Expressions;

namespace Delivery.Infrastructure.Services.Interfaces.Sheduler;

public interface IHangFireService
{
    string Execute<TService>(Expression<Action<TService>> expr, TimeSpan delay);
}