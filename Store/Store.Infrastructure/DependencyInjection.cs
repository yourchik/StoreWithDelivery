using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Store.Application.Services.Interfaces.Integration;
using Store.Application.Settings;
using Store.Domain.Entities;
using Store.Domain.Repositories.Interfaces;
using Store.Infrastructure.Services.Implementations.Integration;
using Store.Infrastructure.Services.Implementations.RabbitMQ;
using Store.Infrastructure.Services.Implementations.Repositories;
using Store.Infrastructure.Services.Implementations.Repositories.EFCoreRepository;
using Store.Infrastructure.Services.Interfaces.RabbitMQ;
using Store.Infrastructure.Settings;

namespace Store.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        //services.Configure<KafkaSettings>(configuration.GetSection("KafkaSettings"));
        services.Configure<RabbitMqSettings>(configuration.GetSection(nameof(RabbitMqSettings)));
        services.Configure<AdminSettings>(configuration.GetSection(nameof(AdminSettings)));
        services.Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)));
        services.AddTransient<IDeliveryService, DeliveryService>();
        services.AddTransient<DataInitializer>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IRabbitMqProducerService, RabbitMqProducerService>();
        services.AddHostedService<RabbitMqConsumerService>();
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("Postgres")));
        services.AddIdentity<User, Role>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
        return services;
    }
}