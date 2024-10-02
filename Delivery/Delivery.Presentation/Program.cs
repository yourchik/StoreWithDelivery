using Delivery.Application;
using Delivery.Application.Middleware;
using Delivery.Infrastructure;
using Delivery.Infrastructure.Services.Implementations.Repositories.EFCoreRepository;
using Delivery.Infrastructure.Services.Implementations.Scheduler;
using Delivery.PresentationApi.Routing;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Чтение конфигурации
builder.Configuration.AddJsonFile("appsettings.json",
        optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json",
        optional: true, reloadOnChange: true);

// Настройка Serilog
builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services));

// Инфраструктура
builder.Services.AddInfrastructure(builder.Configuration);

// Приложение
builder.WebHost.UseUrls("http://localhost:8070");
builder.Services.AddApplication();
builder.Services.AddScoped<RoutesOrdersApi>();
builder.Services.AddScoped<RoutingApi>();

// Добавление Swagger для документации API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Построение приложения
var app = builder.Build();

// Swagger
app.UseSwagger();
app.UseSwaggerUI();

// Настройка Middleware
app.UseMiddleware<ExecutionHandlingMiddleware>(); 

using (var scope = app.Services.CreateScope())
{
    // Регистрация маршрутов
    scope.ServiceProvider.GetRequiredService<RoutingApi>().RegisterAllRoutes(app);

    // Миграции
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();
    
    // Запуск запланированных задач
    scope.ServiceProvider.GetRequiredService<JobSchedulerService>().RegisterJobs();
}

try
{
    Log.Information("Starting Delivery API host");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host Delivery API terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}