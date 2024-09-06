using Store.Application.ModelsDto.Product;
using Store.Application.Services.Factories;
using Store.Application.Services.Implementations.Results;
using Store.Application.Services.Interfaces.Entities;
using Store.Application.Services.Interfaces.Results;
using Store.Domain.Entities;
using Store.Domain.Repositories.Interfaces;
using Store.Domain.Repositories.Utilities;

namespace Store.Application.Services.Implementations.Entities;

public class ProductService(IProductRepository productRepository,
    IProductsCategoryService productsCategoryService)
    : IProductService
{
    public async Task<EntityResult<IEnumerable<Product>>> GetProductsByFilterAsync(BaseFilter<Product> filter, int page, int pageSize)
    {
        var (products, isSuccess, errorMessage) = await productRepository.GetByFilterAsync(filter, page, pageSize); 
        if (!isSuccess)
            return EntityResult<IEnumerable<Product>>.Failure(errorMessage);

        return EntityResult<IEnumerable<Product>>.Success(products);
    }

    public async Task<EntityResult<Product>> GetProductAsync(Guid id)
    {
        var (product, isSuccess, errorMessage) = await productRepository.GetByIdAsync(id);
    
        if (!isSuccess && !string.IsNullOrEmpty(errorMessage))
            return EntityResult<Product>.Failure(errorMessage);
        
        return product != null ? EntityResult<Product>.Success(product) : EntityResult<Product>.Failure("Product not found.");
    }
    public async Task<EntityResult<Product>> CreateProductAsync(CreateProductDto productProductDto)
    {
        var productResults = (await productProductDto.Categories
            .ToAsyncEnumerable()
            .SelectAwait(async productGuid => await productsCategoryService.GetProductsCategoryByFilterAsync(productGuid))
            .ToListAsync());
        
        var errors = productResults
            .Where(result => !result.IsSuccess)
            .SelectMany(result => result.Errors)
            .ToList();
        
        if (errors.Count != 0)
            return EntityResult<Product>.Failure(errors.ToArray());
        
        var productsCategories = productResults
            .Where(result => result is { IsSuccess: true, Value: not null })
            .Select(result => result.Value!)
            .ToList();
        
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = productProductDto.Name,
            Price = productProductDto.Price,
            Amount = productProductDto.Amount,
            Composition = productProductDto.Сomposition,
            Producer = productProductDto.Producer,
            Categories = productsCategories
        };
        
        var (isSuccess, errorMessage) = await productRepository.AddAsync(product);
        if (!isSuccess)
            return EntityResult<Product>.Failure(errorMessage);
        
        return EntityResult<Product>.Success(product);
    }

    public async Task<IResult> DeleteProductAsync(Guid id)
    {
        var (isSuccess, errorMessage) = await productRepository.DeleteAsync(id);
        if (!isSuccess)
            return ResultFactory.CreateResult(isSuccess, errorMessage);
        return ResultFactory.CreateResult(isSuccess);
    }

    public async Task<IResult> ReductionAmountUpdate(Guid id, int reductionAmount)
    {
        var (isSuccess, errorMessage) = await productRepository.ReductionAmountUpdate(id, reductionAmount);
        if (!isSuccess)
            return ResultFactory.CreateResult(isSuccess, errorMessage);
        
        return ResultFactory.CreateResult(isSuccess);
    }
}