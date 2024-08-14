using Store.Application.Dtos.Product;
using Store.Application.Services.Factories;
using Store.Application.Services.Implementations.Results;
using Store.Application.Services.Interfaces.Entities;
using Store.Application.Services.Interfaces.Results;
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

    public async Task<EntityResult<IEnumerable<Product>>> GetProductsAsync()
    {
        var (products, isSuccess, errorMessage) = await _productRepository.GetAllAsync(); 
        if (!isSuccess)
            return EntityResult<IEnumerable<Product>>.Failure(errorMessage);

        return EntityResult<IEnumerable<Product>>.Success(products);
    }

    public async Task<EntityResult<Product>> GetProductAsync(Guid id)
    {
        var (product, isSuccess, errorMessage) = await _productRepository.GetByIdAsync(id);
    
        if (!isSuccess && !string.IsNullOrEmpty(errorMessage))
            return EntityResult<Product>.Failure(errorMessage);
        
        return product != null ? EntityResult<Product>.Success(product) : EntityResult<Product>.Failure("Product not found.");
    }
    public async Task<EntityResult<Product>> CreateProductAsync(CreateProductDto productProductDto)
    {
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = productProductDto.Name,
            Price = productProductDto.Price
        };
        var (isSuccess, errorMessage) = await _productRepository.AddAsync(product);
        if (!isSuccess)
            return EntityResult<Product>.Failure(errorMessage);
        
        return EntityResult<Product>.Success(product);
    }

    public async Task<IResult> DeleteProductAsync(Guid id)
    {
        var (isSuccess, errorMessage) = await _productRepository.DeleteAsync(id);
        if (!isSuccess)
            return ResultFactory.CreateResult(isSuccess, errorMessage);
        
        return ResultFactory.CreateResult(isSuccess);
    }

    public async Task<IResult> ReductionAmountUpdate(Guid id, int reductionAmount)
    {
        var (isSuccess, errorMessage) = await _productRepository.ReductionAmountUpdate(id, reductionAmount);
        if (!isSuccess)
            return ResultFactory.CreateResult(isSuccess, errorMessage);
        
        return ResultFactory.CreateResult(isSuccess);
    }
}