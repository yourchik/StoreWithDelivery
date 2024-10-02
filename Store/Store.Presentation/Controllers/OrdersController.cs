using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Application.ModelsDto.Orders;
using Store.Application.Services.Interfaces.Entities;
using Store.Domain.Entities;
using Store.Domain.Enums;
using Store.Domain.Repositories.Utilities;

namespace Store.Presentation.Controllers;

[Authorize(Roles = nameof(UserRole.User))]
[ApiController]
[Route("orders")]
public class OrdersController(IOrderService orderService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetOrdersByFilterAsync([FromQuery] BaseFilter<Order> filter, int page, int pageSize)
    {
        var orders = await orderService.GetOrdersByFilterAsync(filter, page, pageSize);
        if(!orders.IsSuccess)
            return StatusCode(StatusCodes.Status500InternalServerError, orders.Errors);
        return Ok(orders.Value);
    }
    

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetOrderAsync(Guid id)
    {
        var order = await orderService.GetOrderAsync(id);
        if (!order.IsSuccess)
            return StatusCode(StatusCodes.Status500InternalServerError, order.Errors);
        return Ok(order.Value);
        
    }
    
    [HttpGet("{id:guid}/status")]
    public async Task<IActionResult> GetOrderStatusAsync(Guid id)
    {
        var orders = await orderService.GetOrderStatusAsync(id);
        if(!orders.IsSuccess)
            return StatusCode(StatusCodes.Status500InternalServerError, orders.Errors);
        return Ok(orders.Value);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateOrderAsync([FromBody] CreateOrderDto orderDto)
    {
        var order = await orderService.CreateOrderAsync(orderDto);
        if (!order.IsSuccess)
            return StatusCode(StatusCodes.Status500InternalServerError, order.Errors);
        
        return Ok(new OutputOrderDto
        {
            Id = order.Value.Id,
            Address = order.Value.Address,
            UserId = order.Value.User.Id,
            Products = order.Value.Products.Select(x => x.Id),
            Status = order.Value.Status
        });
    }
    
    [HttpPatch("{id:guid}/cancel")]
    public async Task<IActionResult> CancelOrderAsync(Guid id)
    {
        var order = await orderService.CancelOrderAsync(id);
        if (!order.IsSuccess)
            return StatusCode(StatusCodes.Status500InternalServerError, order.Errors);

        return Ok(order.IsSuccess);
    }
}