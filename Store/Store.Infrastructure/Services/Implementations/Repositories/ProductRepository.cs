using Store.Domain.Entities;
using Store.Domain.Repositories.Interfaces;
using Store.Infrastructure.Services.Implementations.Repositories.EFCoreRepository;

namespace Store.Infrastructure.Services.Implementations.Repositories;

public class ProductRepository(ApplicationDbContext dbContext) 
    : BaseRepository<Product>(dbContext), IProductRepository
{
    public async Task<(bool IsSuccess, string ErrorMessage)> ReductionAmountUpdate(Guid id, int reductionAmount)
    {
        var product = await DbContext.Products.FindAsync(id);
        if (product == null)
            return (false, $"Product with ID {id} not found.");
        product.Amount -= reductionAmount;
        DbContext.Products.Update(product);
        await DbContext.SaveChangesAsync();
        return (true, string.Empty);
    }

}

