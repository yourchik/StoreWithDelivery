using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Application.ModelsDto.Products;
using Store.Application.Services.Interfaces.Entities;
using Store.Domain.Entities;
using Store.Domain.Enums;
using Store.Domain.Repositories.Utilities;

namespace Store.Presentation.Controllers;

[Authorize(Roles = nameof(UserRole.Manager))]
[ApiController]
[Route("products")]
public class ProductsController(IProductService productService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetProductsByFilterAsync([FromQuery] BaseFilter<Product> filter, int page, int pageSize)
    {
        var products = await productService.GetProductsByFilterAsync(filter, page, pageSize);
        if (!products.IsSuccess)
            return StatusCode(StatusCodes.Status500InternalServerError, products.Errors);
        return Ok(products.Value);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetProductAsync(Guid id)
    {
        var product = await productService.GetProductAsync(id);
        if (!product.IsSuccess)
            return StatusCode(StatusCodes.Status500InternalServerError, product.Errors);
        return Ok(product.Value);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProductAsync([FromBody] CreateProductDto createProduct)
    {
        var product = await productService.CreateProductAsync(createProduct);
        if(!product.IsSuccess)
            return StatusCode(StatusCodes.Status500InternalServerError, product.Errors);
        return Ok(product.Value);
    }

    [HttpPatch("amount/{id:guid}")]
    public async Task<IActionResult> UpdateAmountAsync(Guid id, int amountUpdate)
    {
        var product = await productService.UpdateAmountAsync(id, amountUpdate);
        if(!product.IsSuccess)
            return StatusCode(StatusCodes.Status500InternalServerError, product.Errors);
        return Ok(product.IsSuccess);
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteProductAsync(Guid id)
    {
        var product = await productService.DeleteProductAsync(id);
        if (!product.IsSuccess)
            return StatusCode(StatusCodes.Status500InternalServerError, product.Errors);
        return Ok(product.IsSuccess);
    }   
    
    
}