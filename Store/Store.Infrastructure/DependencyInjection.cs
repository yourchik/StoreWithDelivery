using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Store.Application.Services.Interfaces.Integration;
using Store.Domain.Interfaces;
using Store.Domain.Entities;
using Store.Infrastructure.Services.Implementations.Integration;
using Store.Infrastructure.Services.Implementations.Kafka;
using Store.Infrastructure.Services.Implementations.RabbitMQ;
using Store.Infrastructure.Services.Implementations.Repositories;
using Store.Infrastructure.Services.Implementations.Repositories.EFCoreRepository;
using Store.Infrastructure.Services.Interfaces.Kafka;
using Store.Infrastructure.Services.Interfaces.RabbitMQ;
using Store.Infrastructure.Settings;

namespace Store.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        //services.Configure<KafkaSettings>(configuration.GetSection("KafkaSettings"));
        services.AddTransient<IDeliveryService, DeliveryService>();
        services.Configure<RabbitMqSettings>(configuration.GetSection("RabbitMQSettings"));
        services.AddScoped<IRabbitMqProducerService, RabbitMqProducerService>();
        services.AddHostedService<RabbitMqConsumerService>();
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("Postgres")));
        services.AddIdentity<User, Role>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/Auth/Login";
                options.AccessDeniedPath = "/Auth/AccessDenied";
            });
        
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        return services;
    }
}