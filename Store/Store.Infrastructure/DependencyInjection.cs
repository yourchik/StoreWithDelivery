using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Store.Application.Services.Interfaces.Integration;
using Store.Application.Settings;
using Store.Domain.Entities;
using Store.Domain.Repositories.Interfaces;
using Store.Infrastructure.Services.Implementations.Integration;
using Store.Infrastructure.Services.Implementations.RabbitMQ;
using Store.Infrastructure.Services.Implementations.Repositories;
using Store.Infrastructure.Services.Implementations.Repositories.EFCoreRepository;
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
        services.AddScoped<IProductsCategoryRepository, ProductsCategoryRepository>();
        services.AddScoped<IBasketRepository, BasketRepository>();
        services.AddScoped<IAuditRepository, AuditRepository>();
        services.AddScoped<RabbitMqProducerService>();
        services.AddMassTransit(x =>
        {
            x.AddConsumer<RabbitMqConsumerService>();
            x.UsingRabbitMq((context, cfg) =>
            {
                var rabbitMqSettings = context.GetRequiredService<IOptions<RabbitMqSettings>>().Value;
                cfg.Host(rabbitMqSettings.HostName, rabbitMqSettings.Port, "/", h =>
                {
                    h.Username(rabbitMqSettings.UserName);
                    h.Password(rabbitMqSettings.Password);
                });
                cfg.ReceiveEndpoint(rabbitMqSettings.QueueOrderUpdate, e =>
                {
                    e.Consumer<RabbitMqConsumerService>(context);
                });
            });
        });

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("Postgres")));
        services.AddIdentity<User, Role>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
        return services;
    }
}