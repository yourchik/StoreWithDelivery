using Microsoft.Extensions.DependencyInjection;
using Store.Application.Services.Interfaces.Integration;
using Store.Infrastructure.Services.Implementations.Integration;
using Store.Infrastructure.Services.Implementations.Kafka;
using Store.Infrastructure.Services.Interfaces.Kafka;

namespace Store.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IKafkaProducerService, KafkaProducerService>();
        services.AddHostedService<KafkaConsumerService>();
        services.AddTransient<IDeliveryService, DeliveryService>();
        return services;
    }
}