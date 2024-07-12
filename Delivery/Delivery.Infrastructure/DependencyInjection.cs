using Delivery.Application.ModelsDto;
using Delivery.Application.Services.Interfaces.Integration;
using Delivery.Infrastructure.Services.Implementations.Integration;
using Delivery.Infrastructure.Services.Implementations.Kafka;
using Delivery.Infrastructure.Services.Implementations.Sheduler;
using Delivery.Infrastructure.Services.Implementations.Sheduler.Jobs;
using Delivery.Infrastructure.Services.Interfaces.Kafka;
using Delivery.Infrastructure.Services.Interfaces.Sheduler;
using Delivery.Infrastructure.Settings;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Delivery.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<KafkaSettings>(configuration.GetSection(nameof(KafkaSettings)));
        services.Configure<HangfireSettings>(configuration.GetSection(nameof(HangfireSettings)));
        services.AddHangfire(config =>
            config.UsePostgreSqlStorage(c =>
                c.UseNpgsqlConnection(configuration.GetConnectionString("HangfireConnection"))));  
        services.AddHangfireServer();
        services.AddHostedService<KafkaConsumerService>();
        services.AddScoped<IKafkaProducerService, KafkaProducerService>();
        services.AddScoped<IStoreService, StoreService>();
        services.AddTransient<IHangFireService, HangFireService>();
        services.AddTransient<IJob<Order>, UpdateOrderStatusJob>();
        return services;
    }
}