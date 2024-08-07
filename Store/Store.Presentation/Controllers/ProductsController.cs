using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Application.Dtos.Product;
using Store.Application.Services.Interfaces.Entities;
using Store.Domain.Enums;

namespace Store.Presentation.Controllers;

[Authorize(Roles = nameof(UserRole.Manager))]
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet("GetProducts")]
    public async Task<IActionResult> GetProducts()
    {
        var products = await _productService.GetProductsAsync();
        if (!products.IsSuccess)
            return StatusCode(StatusCodes.Status500InternalServerError, products.Errors);
        return Ok(products.Value);
    }

    [HttpGet("GetProduct/{id}")]
    public async Task<IActionResult> GetProduct(Guid id)
    {
        var product = await _productService.GetProductAsync(id);
        if (!product.IsSuccess)
            return StatusCode(StatusCodes.Status500InternalServerError, product.Errors);
        return Ok(product.Value);
    }

    [HttpPost("CreateProduct")]
    public async Task<IActionResult> CreateProductAsync([FromBody]CreateProductDto createProduct)
    {
        var product = await _productService.CreateProductAsync(createProduct);
        if(!product.IsSuccess)
            return StatusCode(StatusCodes.Status500InternalServerError, product.Errors);
        return Ok(product.Value);
    }

    [HttpDelete("DeleteProduct/{id}")]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        var product = await _productService.DeleteProductAsync(id);
        if (!product.IsSuccess)
            return StatusCode(StatusCodes.Status500InternalServerError, product.Errors);
        return Ok(product.IsSuccess);
    }   
}