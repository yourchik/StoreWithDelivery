using Contracts.Enum;
using Delivery.Application.Middleware.CustomersExeption;
using Delivery.Domain.Entities;
using Delivery.Domain.Repositories;
using Delivery.Infrastructure.Services.Implementations.Repositories.EFCoreRepository;

namespace Delivery.Infrastructure.Services.Implementations.Repositories;

public class OrderRepository(ApplicationDbContext dbContext) : IOrderRepository
{
    public async Task CreateAsync(Order order)
    {
        try
        {
            await dbContext.Orders.AddAsync(order);
            await dbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw new EntityException("Error occurred while creating the order.", e);
        }
    }

    public async Task CreateRangeAsync(IEnumerable<Order> orders)
    {
        try
        {
            await dbContext.Orders.AddRangeAsync(orders);
            await dbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw new EntityException("Error occurred while creating the list of orders.", e);
        }
    }

    public async Task UpdateStatusAsync(Order order, OrderStatus orderStatus)
    {
        if (order == null)
            throw new ArgumentNullException(nameof(order));

        if (order.Status is OrderStatus.Cancelled or OrderStatus.Delivered)
            throw new InvalidOperationException($"Cannot update order status. The order has already been {order.Status.ToString()}.");

        order.Status = orderStatus;
        await dbContext.SaveChangesAsync();
    }

    public async Task<Order> GetOrderAsync(Guid id)
    {
        var order = await dbContext.Orders.FindAsync(id);
        if (order == null)
            throw new KeyNotFoundException($"Order with ID {id} was not found.");
        
        return order; 
    }
}