using Microsoft.AspNetCore.Mvc;
using Store.Application.Services.Interfaces.Entities;
using Store.Domain.Entities;

namespace Store.Presentation.Controllers;

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
    public async Task<OkObjectResult> GetProducts()
    {
        var products = await _productService.GetProductsAsync();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct(Guid id)
    {
        var product = await _productService.GetProductAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        return Ok(product);
    }

    [HttpPost]
    public async Task<CreatedAtActionResult> AddProduct([FromBody]Product product)
    {
        await _productService.AddProductAsync(product);
        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
    }

    [HttpDelete("{id}")]
    public async Task<NoContentResult> DeleteProduct(Guid guid)
    {
        await _productService.DeleteProductAsync(guid);
        return NoContent();
    }   
}