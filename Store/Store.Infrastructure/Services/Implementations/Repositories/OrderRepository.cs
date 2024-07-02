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

    public async Task<IEnumerable<Order>> GetAllAsync() =>
        await _dbContext.Orders.ToListAsync();

    public async Task<Order?> GetByIdAsync(Guid id) =>
        await _dbContext.Orders.FindAsync(id);
    
    public async Task Add(Order entity)
    {
        await _dbContext.Orders.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var order = await _dbContext.Orders.FindAsync(id);
        if (order != null)
        {
            _dbContext.Orders.Remove(order);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task UpdateStatusAsync(Guid id, OrderStatus status)
    {
        var order = await _dbContext.Orders.FindAsync(id);
        if (order != null)
        {
            order.Status = status;
            await _dbContext.SaveChangesAsync();
        }
    }
}