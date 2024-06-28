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

    public async Task<IEnumerable<Product>> GetProducts() =>
        await _productRepository.GetAll();

    public async Task<Product?> GetProduct(int id) =>
        await _productRepository.GetById(id);

    public async Task AddProduct(Product product) =>
        await _productRepository.Add(product);

    public async Task DeleteProduct(int id) =>
        await _productRepository.Delete(id);
}