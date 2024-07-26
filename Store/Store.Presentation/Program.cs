using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Store.Application;
using Store.Infrastructure;
using Store.Infrastructure.Services.Implementations.Repositories.EFCoreRepository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Configuration.AddEnvironmentVariables();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "StoreApi", Version = "v1" });
});

// Application
builder.Services.AddInfrastructure(builder.Configuration);

// Integration
builder.Services.AddApplication();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "StoreApi V1");
});

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();