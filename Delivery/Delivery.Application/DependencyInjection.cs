using Delivery.Application.Services.Implementations.Orders;
using Delivery.Application.Services.Interfaces.Orders;
using Microsoft.Extensions.DependencyInjection;

namespace Delivery.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IOrderService, OrderService>();
        return services;
    }
}