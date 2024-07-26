using Microsoft.EntityFrameworkCore;
using Store.Domain.Entities;
using Store.Domain.Interfaces;
using Store.Infrastructure.Services.Implementations.Repositories.EFCoreRepository;

namespace Store.Infrastructure.Services.Implementations.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly ApplicationDbContext _dbContext;

    public OrderRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<(IEnumerable<Order> Entities, bool IsSuccess, string ErrorMessage)> GetAllAsync()
    {
        var orders = await _dbContext.Orders.ToListAsync();
        if (orders.Count == 0)
            return (new List<Order>(), false, string.Empty);    
        return (orders, true, $"No orders found.");
    }

    public async Task<(Order? Entity, bool IsSuccess, string ErrorMessage)> GetByIdAsync(Guid id)
    {
        var order = await _dbContext.Orders.FindAsync(id);
        if (order == null)
            return (null, false, $"Order with ID {id} not found.");

        return (order, true, string.Empty);
    }
        
    
    public async Task<(bool IsSuccess, string ErrorMessage)> AddAsync(Order entity)
    {
        await _dbContext.Orders.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
        return (true, string.Empty);
    }

    public async Task<(bool IsSuccess, string ErrorMessage)> DeleteAsync(Guid id)
    {
        var order = await _dbContext.Orders.FindAsync(id);
        if (order == null)
            return (false, $"Order with ID {id} not found.");

        _dbContext.Orders.Remove(order);
        await _dbContext.SaveChangesAsync();
        return (true, string.Empty);
    }

    public async Task<(bool IsSuccess, string ErrorMessage)> UpdateStatusAsync(Guid id, OrderStatus status)
    {
        var order = await _dbContext.Orders.FindAsync(id);
        if (order == null)
            return (false, $"Order with ID {id} not found.");
        
        order.Status = status;
        await _dbContext.SaveChangesAsync();
        return (true, string.Empty);
    }
}