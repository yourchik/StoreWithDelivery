using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Application.Services.Interfaces.Entities;
using Store.Domain.Enums;

namespace Store.Presentation.Controllers;

[Authorize(Roles = nameof(UserRole.User))]
[ApiController]
[Route("api/[controller]")]
public class BasketController(IBasketService basketService) : ControllerBase
{
    [HttpPost("products/{id:guid}")]
    public async Task<IActionResult> AddProductToBasketAsync(Guid id)
    {
        var basket = await basketService.AddProduct(id);
        if (!basket.IsSuccess)
            return StatusCode(StatusCodes.Status500InternalServerError, basket.Errors);
        
        return Ok(basket.Value);
    }
    
    [HttpDelete("products/{id:guid}")]
    public async Task<IActionResult> DeleteProductToBasketAsync(Guid id)
    {
        var basket = await basketService.AddProduct(id);
        if (!basket.IsSuccess)
            return StatusCode(StatusCodes.Status500InternalServerError, basket.Errors);
        
        return Ok(basket.Value);
    }

    [HttpGet]
    public async Task<IActionResult> GetOrCreateBasketAsync()
    {
        var basket = await basketService.GetOrCreate();
        if (!basket.IsSuccess)
            return StatusCode(StatusCodes.Status500InternalServerError, basket.Errors);
        
        return Ok(basket.Value);
    }
    
    [HttpDelete]
    public async Task<IActionResult> ClearBasketAsync()
    {
        var basket = await basketService.ClearBasket();
        if (!basket.IsSuccess)
            return StatusCode(StatusCodes.Status500InternalServerError, basket.Errors);
        
        return Ok(basket.IsSuccess);
    }
    
}