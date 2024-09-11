using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Application.ModelsDto.Product;
using Store.Application.Services.Interfaces.Entities;
using Store.Domain.Entities;
using Store.Domain.Enums;
using Store.Domain.Repositories.Utilities;

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

    [HttpGet]
    public async Task<IActionResult> GetProductsByFilterAsync(BaseFilter<Product> filter, int page, int pageSize)
    {
        var products = await _productService.GetProductsByFilterAsync(filter, page, pageSize);
        if (!products.IsSuccess)
            return StatusCode(StatusCodes.Status500InternalServerError, products.Errors);
        return Ok(products.Value);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetProductAsync(Guid id)
    {
        var product = await _productService.GetProductAsync(id);
        if (!product.IsSuccess)
            return StatusCode(StatusCodes.Status500InternalServerError, product.Errors);
        return Ok(product.Value);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProductAsync([FromBody]CreateProductDto createProduct)
    {
        var product = await _productService.CreateProductAsync(createProduct);
        if(!product.IsSuccess)
            return StatusCode(StatusCodes.Status500InternalServerError, product.Errors);
        return Ok(product.Value);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteProductAsync(Guid id)
    {
        var product = await _productService.DeleteProductAsync(id);
        if (!product.IsSuccess)
            return StatusCode(StatusCodes.Status500InternalServerError, product.Errors);
        return Ok(product.IsSuccess);
    }   
}