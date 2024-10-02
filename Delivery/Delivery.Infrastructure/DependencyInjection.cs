using Delivery.Application.Services.Interfaces.Integration;
using Delivery.Domain.Repositories;
using Delivery.Infrastructure.Services.Implementations.Integration;
using Delivery.Infrastructure.Services.Implementations.RabbitMQ;
using Delivery.Infrastructure.Services.Implementations.Repositories;
using Delivery.Infrastructure.Services.Implementations.Repositories.EFCoreRepository;
using Delivery.Infrastructure.Services.Implementations.Scheduler;
using Delivery.Infrastructure.Services.Implementations.Scheduler.Jobs;
using Delivery.Infrastructure.Services.Interfaces.RabbitMQ;
using Delivery.Infrastructure.Services.Interfaces.Scheduler;
using Delivery.Infrastructure.Settings;
using Hangfire;
using Hangfire.PostgreSql;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Delivery.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Настройки
        services.Configure<RabbitMqSettings>(configuration.GetSection(nameof(RabbitMqSettings)));
        services.Configure<HangfireCronSettings>(configuration.GetSection(nameof(HangfireCronSettings)));
        //services.Configure<KafkaSettings>(configuration.GetSection(nameof(KafkaSettings)));
        
        // БД
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("Postgres")));
        
        // Hangfire
        services.AddHangfire(config =>
            config.UsePostgreSqlStorage(c =>
                c.UseNpgsqlConnection(configuration.GetConnectionString("Postgres"))));
        services.AddHangfireServer();
        
        // Сервисы
        services.AddScoped<IRabbitMqMessageService, RabbitMqMessageService>();
        services.AddScoped<IStoreService, StoreService>();
        services.AddScoped<IOrderRepository, OrderRepository>();
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

        // Hangfire и джобы
        services.AddTransient<IHangFireService, HangFireService>();
        services.AddTransient<IJob, GetNewOrdersJob>();

        // Регистрация JobSchedulerService
        services.AddSingleton<JobSchedulerService>();
        
        return services;
    }
}