using Delivery.Application.Services.Implementations.Orders;
using Delivery.Application.Services.Interfaces.Orders;
using Delivery.Infrastructure.Services.Implementations.Kafka;
using Delivery.Infrastructure.Services.Interfaces.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        config.AddEnvironmentVariables();
    })
    .ConfigureServices((context, services) =>
    {
        services.AddSingleton<IKafkaConsumerService, KafkaConsumerService>();
        services.AddSingleton<IKafkaProducerService, KafkaProducerService>();
        services.AddScoped<IOrderService, OrderService>();
    })
    .ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        logging.AddConsole();
    })
    .Build();

await host.RunAsync();