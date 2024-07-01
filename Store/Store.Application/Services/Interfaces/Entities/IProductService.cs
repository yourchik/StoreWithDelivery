using Store.Domain.Entities;

namespace Store.Application.Services.Interfaces.Entities;

public interface IProductService
{
    Task<IEnumerable<Product>> GetProductsAsync();
    Task<Product?> GetProductAsync(Guid id);
    Task AddProductAsync(Product product);
    Task DeleteProductAsync(Guid id);
}