using Delivery.Application.Services.Interfaces.Integration;
using Delivery.Infrastructure.Services.Implementations.Integration;
using Delivery.Infrastructure.Services.Implementations.Kafka;
using Delivery.Infrastructure.Services.Interfaces.Kafka;
using Delivery.Infrastructure.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Delivery.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<KafkaSettings>(configuration.GetSection("KafkaSettings"));
        services.AddHostedService<KafkaConsumerService>();
        services.AddScoped<IKafkaProducerService, KafkaProducerService>();
        services.AddScoped<IStoreService, StoreService>();
        return services;
    }
}