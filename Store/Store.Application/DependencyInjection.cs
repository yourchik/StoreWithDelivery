﻿using Microsoft.Extensions.DependencyInjection;
using Store.Application.Services.Implementations.Entities;
using Store.Application.Services.Interfaces.Entities;

namespace Store.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IProductService, ProductService>();
        return services;
    }
}