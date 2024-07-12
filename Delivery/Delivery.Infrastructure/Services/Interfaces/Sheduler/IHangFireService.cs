using System.Linq.Expressions;
using Delivery.Application.ModelsDto;

namespace Delivery.Infrastructure.Services.Interfaces.Sheduler;

public interface IHangFireService
{
    string Execute<TService>(Expression<Action<TService>> expr, TimeSpan delay);
}