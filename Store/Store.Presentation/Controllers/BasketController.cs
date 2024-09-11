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
    [HttpPost("AddProductToBasket")]
    public async Task<IActionResult> AddProductToBasket(Guid id)
    {
        var basket = await basketService.AddProduct(id);
        if (!basket.IsSuccess)
            return StatusCode(StatusCodes.Status500InternalServerError, basket.Errors);
        
        return Ok(basket.Value);
    }
    
    [HttpPost("DeleteProductToBasket")]
    public async Task<IActionResult> DeleteProductToBasket(Guid id)
    {
        var basket = await basketService.AddProduct(id);
        if (!basket.IsSuccess)
            return StatusCode(StatusCodes.Status500InternalServerError, basket.Errors);
        
        return Ok(basket.Value);
    }

    [HttpGet("GetBasketOrCreate")]
    public async Task<IActionResult> GetOrCreateBasket()
    {
        var basket = await basketService.GetOrCreate();
        if (!basket.IsSuccess)
            return StatusCode(StatusCodes.Status500InternalServerError, basket.Errors);
        
        return Ok(basket.Value);
    }
    
    [HttpGet("ClearBasket")]
    public async Task<IActionResult> ClearBasket()
    {
        var basket = await basketService.ClearBasket();
        if (!basket.IsSuccess)
            return StatusCode(StatusCodes.Status500InternalServerError, basket.Errors);
        
        return Ok(basket.IsSuccess);
    }
    
}