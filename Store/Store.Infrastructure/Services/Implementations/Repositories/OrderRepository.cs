using Store.Domain.Entities;
using Store.Domain.Repositories.Interfaces;
using Store.Infrastructure.Services.Implementations.Repositories.EFCoreRepository;

namespace Store.Infrastructure.Services.Implementations.Repositories;

public class OrderRepository(ApplicationDbContext dbContext) : BaseRepository<Order>(dbContext), IOrderRepository
{
    public async Task<(bool IsSuccess, string ErrorMessage)> UpdateStatusAsync(Guid id, OrderStatus status)
    {
        var order = await DbContext.Orders.FindAsync(id);
        if (order == null)
            return (false, $"Order with ID {id} not found.");
        
        order.Status = status;
        await DbContext.SaveChangesAsync();
        return (true, string.Empty);
    }
}