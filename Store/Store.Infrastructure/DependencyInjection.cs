using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Store.Application.Services.Interfaces.Integration;
using Store.Domain.Interfaces;
using Store.Domain.Entities;
using Store.Infrastructure.Services.Implementations.Integration;
using Store.Infrastructure.Services.Implementations.Kafka;
using Store.Infrastructure.Services.Implementations.Repositories;
using Store.Infrastructure.Services.Implementations.Repositories.EFCoreRepository;
using Store.Infrastructure.Services.Interfaces.Kafka;
using Store.Infrastructure.Settings;

namespace Store.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<KafkaSettings>(configuration.GetSection("KafkaSettings"));
        services.AddSingleton<IKafkaProducerService, KafkaProducerService>();
        services.AddHostedService<KafkaConsumerService>();
        services.AddTransient<IDeliveryService, DeliveryService>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddIdentity<User, Role>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
        return services;
    }
}