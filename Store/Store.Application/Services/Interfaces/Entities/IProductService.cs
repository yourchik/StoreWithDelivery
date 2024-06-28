using Store.Domain.Entities;

namespace Store.Application.Services.Interfaces.Entities;

public interface IProductService
{
    Task<IEnumerable<Product>> GetProducts();
    Task<Product?> GetProduct(int id);
    Task AddProduct(Product product);
    Task DeleteProduct(int id);
}