using Microsoft.EntityFrameworkCore;
using Store.Domain.Entities;
using Store.Domain.Interfaces;
using Store.Infrastructure.Services.Repositories.EFCoreRepository;

namespace Store.Infrastructure.Services.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ProductRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Product>> GetAllAsync() => await _dbContext.Products.ToListAsync();

    public async Task<Product?> GetByIdAsync(Guid id) => await _dbContext.Products.FindAsync(id);

    public async Task Add(Product entity)
    {
        await _dbContext.Products.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var product = await _dbContext.Products.FindAsync(id);
        if (product != null)
        {
            _dbContext.Products.Remove(product);
            await _dbContext.SaveChangesAsync();
        }
    }
}