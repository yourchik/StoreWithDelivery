using Store.Application.Services.Interfaces.Entities;
using Store.Domain.Entities;
using Store.Domain.Repositories.Interfaces;
using Store.Infrastructure.Services.Implementations.Repositories.EFCoreRepository;

namespace Store.Infrastructure.Services.Implementations.Repositories;

public class BasketRepository(ApplicationDbContext dbContext) : BaseRepository<Basket>(dbContext), IBasketRepository
{
    public async Task<(bool IsSuccess, string ErrorMessage)> UpdateAsync(Basket basketUpdate)
    {
        var basket = await DbContext.Baskets.FindAsync(basketUpdate.Id);
        if (basket == null)
            return (false, $"Basket with ID {basketUpdate.Id} not found.");

        basket.Products = basketUpdate.Products;
        await DbContext.SaveChangesAsync();
        return (true, string.Empty);
    }

    public async Task<(Basket Entity, bool IsSuccess, string ErrorMessage)> GetByUserIdAsync(Guid userId)
    {
        var basket = await DbContext.Baskets.FindAsync(userId);
        if (basket == null)
            return (default!, false, $"Basket with UserId {userId} not found.");

        return (basket, true, string.Empty);
    }
}