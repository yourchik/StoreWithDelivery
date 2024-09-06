using Store.Application.ModelsDto.Product;
using Store.Application.Services.Implementations.Results;
using Store.Application.Services.Interfaces.Results;
using Store.Domain.Entities;
using Store.Domain.Repositories.Utilities;

namespace Store.Application.Services.Interfaces.Entities;

public interface IProductsCategoryService
{
    Task<EntityResult<IEnumerable<ProductsCategory>>> GetProductsCategoryByFilterAsync(BaseFilter<ProductsCategory> filter, int page, int pageSize);
    Task<EntityResult<ProductsCategory>> GetProductsCategoryByFilterAsync(Guid id);
    Task<EntityResult<ProductsCategory>> CreateProductsCategoryAsync(CreateProductsCategoryDto createProduct);
    Task<IResult> DeleteProductsCategoryAsync(Guid id);
}