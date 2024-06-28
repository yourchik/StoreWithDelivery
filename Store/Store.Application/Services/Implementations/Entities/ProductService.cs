using Store.Application.Services.Interfaces.Entities;
using Store.Domain.Entities;
using Store.Domain.Interfaces;

namespace Store.Application.Services.Implementations.Entities;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<Product>> GetProductsAsync() =>
        await _productRepository.GetAllAsync();

    public async Task<Product?> GetProductAsync(int id) =>
        await _productRepository.GetByIdAsync(id);

    public async Task AddProductAsync(Product product) =>
        await _productRepository.Add(product);

    public async Task DeleteProductAsync(int id) =>
        await _productRepository.DeleteAsync(id);
}