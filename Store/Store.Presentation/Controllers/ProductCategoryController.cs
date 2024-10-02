using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Application.ModelsDto.Products;
using Store.Application.Services.Interfaces.Entities;
using Store.Domain.Entities;
using Store.Domain.Enums;
using Store.Domain.Repositories.Utilities;

namespace Store.Presentation.Controllers;

[ApiController]
[Route("productCategory")]
[Authorize(Roles = nameof(UserRole.Manager))]
public class ProductCategoryController(IProductsCategoryService productsCategoryService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetCategoriesAsync([FromQuery] BaseFilter<ProductsCategory> filter, int page, int pageSize)
    {
        var productsCategories = await productsCategoryService.GetProductsCategoryAsync(filter, page, pageSize);
        if (!productsCategories.IsSuccess)
            return BadRequest(productsCategories.Errors);

        return Ok(productsCategories.Value);
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetCategory(Guid id)
    {
        var productsCategory = await productsCategoryService.GetProductsCategoryAsync(id);
        if (!productsCategory.IsSuccess)
            return NotFound(productsCategory.Errors);

        return Ok(productsCategory.Value);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateCategoryAsync([FromBody] CreateProductsCategoryDto createCategoryDto)
    {
        var productsCategory = await productsCategoryService.CreateProductsCategoryAsync(createCategoryDto);
        if (!productsCategory.IsSuccess)
            return StatusCode(StatusCodes.Status500InternalServerError, productsCategory.Errors);
        
        return Ok(productsCategory.Value);
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteCategoryAsync(Guid id)
    {
        var result = await productsCategoryService.DeleteProductsCategoryAsync(id);
        if (!result.IsSuccess)
            return NotFound(result.Errors);

        return NoContent();
    }
}
