﻿using Store.Application.ModelsDto.Products;
using Store.Application.Services.Factories;
using Store.Application.Services.Implementations.Results;
using Store.Application.Services.Interfaces.Entities;
using Store.Application.Services.Interfaces.Results;
using Store.Domain.Entities;
using Store.Domain.Repositories.Interfaces;
using Store.Domain.Repositories.Utilities;

namespace Store.Application.Services.Implementations.Entities;

public class ProductsCategoryService(IProductsCategoryRepository productsCategoryRepository)
    : IProductsCategoryService
{
    public async Task<EntityResult<IEnumerable<ProductsCategory>>> GetProductsCategoryAsync(BaseFilter<ProductsCategory> filter, int page, int pageSize)
    {
        var (products, isSuccess, errorMessage) = await productsCategoryRepository.GetByFilterAsync(filter, page, pageSize); 
        if (!isSuccess)
            return EntityResult<IEnumerable<ProductsCategory>>.Failure(errorMessage);

        return EntityResult<IEnumerable<ProductsCategory>>.Success(products);
    }

    public async Task<EntityResult<ProductsCategory>> GetProductsCategoryAsync(Guid id)
    {
        var (product, isSuccess, errorMessage) = await productsCategoryRepository.GetByIdAsync(id);
    
        if (!isSuccess && !string.IsNullOrEmpty(errorMessage))
            return EntityResult<ProductsCategory>.Failure(errorMessage);
        
        return EntityResult<ProductsCategory>.Success(product);
    }

    public async Task<EntityResult<ProductsCategory>> CreateProductsCategoryAsync(CreateProductsCategoryDto productProductDto)
    {
        var productsCategory = new ProductsCategory
        {
            Id = Guid.NewGuid(),
            Name = productProductDto.Name,
        };
        var (isSuccess, errorMessage) = await productsCategoryRepository.CreateAsync(productsCategory);
        if (!isSuccess)
            return EntityResult<ProductsCategory>.Failure(errorMessage);
        
        return EntityResult<ProductsCategory>.Success(productsCategory);
    }

    public async Task<IResult> DeleteProductsCategoryAsync(Guid id)
    {
        var (isSuccess, errorMessage) = await productsCategoryRepository.DeleteAsync(id);
        if (!isSuccess)
            return ResultFactory.CreateResult(isSuccess, errorMessage);
        
        return ResultFactory.CreateResult(isSuccess);
    }
}