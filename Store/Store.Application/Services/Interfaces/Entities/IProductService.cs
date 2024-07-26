using Store.Application.Dtos.Product;
using Store.Application.Services.Implementations.Results;
using Store.Application.Services.Interfaces.Results;
using Store.Domain.Entities;

namespace Store.Application.Services.Interfaces.Entities;

public interface IProductService
{
    Task<EntityResult<IEnumerable<Product>>> GetProductsAsync();
    Task<EntityResult<Product>> GetProductAsync(Guid id);
    Task<EntityResult<Product>> CreateProductAsync(CreateProductDto createProduct);
    Task<IResult> DeleteProductAsync(Guid id);
}