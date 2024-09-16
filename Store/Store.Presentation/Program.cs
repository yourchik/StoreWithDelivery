using Store.Application;
using Store.Application.Middleware;
using Store.Infrastructure;
using Store.Infrastructure.Services.Implementations.Repositories.EFCoreRepository;
using Store.Presentation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Configuration.AddJsonFile("appsettings.json",
    optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json",
        optional: true, reloadOnChange: true);
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddSwaggerDocumentation();

// Infrastructure
builder.Services.AddInfrastructure(builder.Configuration);

// Application
builder.Services.AddApplication(builder.Configuration);

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "StoreApi V1");
});

app.UseAuthentication();
app.UseMiddleware<AdminAccessMiddleware>();
app.UseAuthorization();
app.UseMiddleware<ExecutionHandlingMiddleware>();
app.MapControllers();

var dataInitializer = app.Services.GetRequiredService<DataInitializer>();
await dataInitializer.InitializeAsync();

app.Run();
