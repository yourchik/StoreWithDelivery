using Microsoft.EntityFrameworkCore;
using Serilog; 
using Store.Application; 
using Store.Application.Middleware; 
using Store.Infrastructure; 
using Store.Infrastructure.Services.Implementations.Repositories.EFCoreRepository; 
using Store.Presentation; 

var builder = WebApplication.CreateBuilder(args); 

// Добавление служб
builder.Services.AddControllers(); 
builder.Services.AddEndpointsApiExplorer(); 
builder.Configuration.AddJsonFile("appsettings.json", 
        optional: false, reloadOnChange: true) 
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", 
        optional: true, reloadOnChange: true); 
builder.Configuration.AddEnvironmentVariables(); 
builder.WebHost.UseUrls("http://localhost:8070");

// Конфигурация Serilog
builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration) 
    .ReadFrom.Services(services)); 

// Регистрация Swagger
builder.Services.AddSwaggerDocumentation(); 

// Инфраструктура
builder.Services.AddInfrastructure(builder.Configuration); 

// Приложение
builder.Services.AddApplication(builder.Configuration); 

var app = builder.Build(); 

// Настройка Swagger
app.UseSwagger(); 
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "StoreApi V1"); 
});

// Настройка Middleware
app.UseAuthentication(); 
app.UseMiddleware<AdminAccessMiddleware>(); 
app.UseAuthorization(); 
app.UseMiddleware<ExecutionHandlingMiddleware>(); 

// Маршрутизация
app.MapControllers(); 

using (var scope = app.Services.CreateScope())
{
    // Миграции
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();

    // Инициализация данных
    var dataInitializer = scope.ServiceProvider.GetRequiredService<DataInitializer>();
    await dataInitializer.InitializeAsync();
}

try
{
    Log.Information("Starting Store API host"); 
    app.Run(); 
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host Store API terminated unexpectedly"); 
}
finally
{
    Log.CloseAndFlush(); 
}