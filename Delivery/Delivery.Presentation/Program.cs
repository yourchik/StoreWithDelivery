using Delivery.Application;
using Delivery.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

var host = Host.CreateDefaultBuilder()
    .ConfigureAppConfiguration((context, config) =>
    {
        config.AddJsonFile("appsettings.json",
                optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json",
                optional: true, reloadOnChange: true);
        config.AddEnvironmentVariables();
    })
    .UseSerilog((context, services, configuration) =>
    {
        configuration
            .ReadFrom.Configuration(context.Configuration)
            .Enrich.FromLogContext();
    })
    .ConfigureServices((context, services) =>
    {
        services.AddInfrastructure(context.Configuration);
        services.AddApplication();
    })
    .ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        logging.AddConsole();
    })
    .Build();

try
{
    Log.Information("Starting Delivery host");
    await host.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host Delivery terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}