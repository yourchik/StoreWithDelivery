using Store.Application.Services.Factories;
using Store.Application.Services.Implementations.Results;
using Store.Application.Services.Interfaces.Entities;
using Store.Application.Services.Interfaces.Results;
using Store.Domain.Entities;
using Store.Domain.Repositories.Interfaces;

namespace Store.Application.Services.Implementations.Entities;

public class BasketService(
    IBasketRepository basketRepository,
    IUserService userService,
    IProductService productService) 
    : IBasketService
{
    public async Task<EntityResult<Basket>> GetOrCreate()
    {
        var user = await userService.GetCurrentUserAsync();
        var basketResult = await GetUserBasketAsync(user);
        if (basketResult.IsSuccess)
            return basketResult;
        
        var newBasket = new Basket { User = user };
        var (isSuccess, errorMessage) = await basketRepository.CreateAsync(newBasket);
        if (!isSuccess)
            return EntityResult<Basket>.Failure(errorMessage);

        return EntityResult<Basket>.Success(newBasket);
    }

    public async Task<EntityResult<Basket>> AddProduct(Guid productId)
    {
        var basket = await GetOrCreate();
        if (!basket.IsSuccess)
            return EntityResult<Basket>.Failure(basket.Errors.ToArray());
        
        var product = await productService.GetProductAsync(productId);
        if (!product.IsSuccess)
            return EntityResult<Basket>.Failure(product.Errors.ToArray());

        if (product.Value.Amount == 0)
            return EntityResult<Basket>.Failure($"The product '{product.Value.Name}' is out of stock.");
        
        basket.Value.Products.Add(product.Value);
        var (isSuccess, errorMessage) = await basketRepository.UpdateAsync(basket.Value);
        if (!isSuccess)
            return EntityResult<Basket>.Failure(errorMessage);

        return EntityResult<Basket>.Success(basket.Value);
    }
    
    public async Task<EntityResult<Basket>> DeleteProduct(Guid productId)
    {
        var user = await userService.GetCurrentUserAsync(); 
        var basketResult = await GetUserBasketAsync(user);
        if (!basketResult.IsSuccess)
            return basketResult;
        
        var product = await productService.GetProductAsync(productId);
        if (!product.IsSuccess)
            return EntityResult<Basket>.Failure(product.Errors.ToArray());

        var productInBasket = basketResult.Value.Products.FirstOrDefault(p => p.Id == product.Value.Id);

        if (productInBasket == null)
            return EntityResult<Basket>.Failure($"Product {product.Value.Id} is not found in the basket.");
        
        basketResult.Value.Products.Remove(product.Value);
        var (isSuccess, errorMessage) = await basketRepository.UpdateAsync(basketResult.Value);
        if (!isSuccess)
            return EntityResult<Basket>.Failure(errorMessage);

        return EntityResult<Basket>.Success(basketResult.Value);
        
    }

    public async Task<IResult> ClearBasket()
    {
        var user = await userService.GetCurrentUserAsync();
        var basketResult = await GetUserBasketAsync(user);
        if (!basketResult.IsSuccess)
            return ResultFactory.CreateResult(basketResult.IsSuccess, basketResult.Errors.ToArray());
        
        var basket = basketResult.Value;
        basket.Products.Clear();
        
        var updateResult = await basketRepository.UpdateAsync(basket);
        if (!updateResult.IsSuccess)
            return ResultFactory.CreateResult(basketResult.IsSuccess, basketResult.Errors.ToArray());

        return ResultFactory.CreateResult(basketResult.IsSuccess, string.Empty);
    }
    
    private async Task<EntityResult<Basket>> GetUserBasketAsync(User user)
    {
        var (basket, isSuccess, errorMessage) = await basketRepository.GetByUserIdAsync(user.Id);
        if (!isSuccess)
            return EntityResult<Basket>.Failure(errorMessage);

        return EntityResult<Basket>.Success(basket);
    }
}