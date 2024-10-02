using Store.Application.ModelsDto.Products;
using Store.Application.Services.Implementations.Results;
using Store.Application.Services.Interfaces.Results;
using Store.Domain.Entities;
using Store.Domain.Repositories.Utilities;

namespace Store.Application.Services.Interfaces.Entities;

public interface IProductsCategoryService
{
    Task<EntityResult<IEnumerable<ProductsCategory>>> GetProductsCategoryAsync(BaseFilter<ProductsCategory> filter, int page, int pageSize);
    Task<EntityResult<ProductsCategory>> GetProductsCategoryAsync(Guid id);
    Task<EntityResult<ProductsCategory>> CreateProductsCategoryAsync(CreateProductsCategoryDto createProduct);
    Task<IResult> DeleteProductsCategoryAsync(Guid id);
}