using Store.Application.Dtos.ProductDtos;
using Store.Application.Services.Implementations.Results;
using Store.Application.Services.Interfaces.Results;
using Store.Domain.Entities;
using Store.Domain.Repositories.Utilities;

namespace Store.Application.Services.Interfaces.Entities;

public interface IProductService
{
    Task<EntityResult<IEnumerable<Product>>> GetProductsByFilterAsync(BaseFilter<Product> filter, int page, int pageSize);
    Task<EntityResult<Product>> GetProductAsync(Guid id);
    Task<EntityResult<Product>> CreateProductAsync(CreateProductDto createProduct);
    Task<IResult> DeleteProductAsync(Guid id);
    Task<IResult> ReductionAmountUpdate(Guid id, int reductionAmount);

}