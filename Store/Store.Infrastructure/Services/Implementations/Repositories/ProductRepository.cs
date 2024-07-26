using Microsoft.EntityFrameworkCore;
using Store.Domain.Entities;
using Store.Domain.Interfaces;
using Store.Infrastructure.Services.Implementations.Repositories.EFCoreRepository;

namespace Store.Infrastructure.Services.Implementations.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ProductRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<(IEnumerable<Product> Entities, bool IsSuccess, string ErrorMessage)> GetAllAsync()
    {
        var products = await _dbContext.Products.ToListAsync();
        if (products.Count == 0)
            return (products, false, $"Order not found.");
        return (products, true, string.Empty);
    }

    public async Task<(Product? Entity, bool IsSuccess, string ErrorMessage)> GetByIdAsync(Guid id)
    {
        var product = await _dbContext.Products.FindAsync(id);
        if (product == null)
            return (null, false, $"Product with ID {id} not found.");
        return (product, true, string.Empty);
    }

    public async Task<(bool IsSuccess, string ErrorMessage)> AddAsync(Product entity)
    {
        await _dbContext.Products.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
        return (true, string.Empty);
    }

    public async Task<(bool IsSuccess, string ErrorMessage)> DeleteAsync(Guid id)
    {
        var product = await _dbContext.Products.FindAsync(id);
        if (product == null)
            return (false, $"Product with ID {id} not found.");
        _dbContext.Products.Remove(product);
        await _dbContext.SaveChangesAsync();
        return (true, string.Empty);
    }
}

