using Microsoft.EntityFrameworkCore;
using Store.Domain.Interfaces;
using Store.Application.Services.Implementations.Entities;
using Store.Application.Services.Interfaces.Entities;
using Store.Application.Services.Interfaces.Kafka;
using Store.Infrastructure.Services.Kafka;
using Store.Infrastructure.Services.Repositories;
using Store.Infrastructure.Services.Repositories.EFCoreRepository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("ConnectionString")));

// Repositories
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

// Services
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IProductService, ProductService>();

// Kafka
builder.Services.AddSingleton<IKafkaProducer, KafkaProducer>();
builder.Services.AddSingleton<IKafkaConsumer, KafkaConsumer>();

// Add controllers
builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();