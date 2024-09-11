using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Application.ModelsDto.Order;
using Store.Application.Services.Interfaces.Entities;
using Store.Domain.Entities;
using Store.Domain.Enums;
using Store.Domain.Repositories.Utilities;

namespace Store.Presentation.Controllers;

[Authorize(Roles = nameof(UserRole.User))]
[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    public async Task<IActionResult> GetOrdersByFilterAsync(BaseFilter<Order> filter, int page, int pageSize)
    {
        var orders = await _orderService.GetOrdersByFilterAsync(filter, page, pageSize);
        if(!orders.IsSuccess)
            return StatusCode(StatusCodes.Status500InternalServerError, orders.Errors);
        return Ok(orders.Value);
    }
    

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetOrderAsync(Guid id)
    {
        var order = await _orderService.GetOrderAsync(id);
        if (!order.IsSuccess)
            return StatusCode(StatusCodes.Status500InternalServerError, order.Errors);
        return Ok(order.Value);
        
    }
    
    [HttpGet("{id:guid}/status")]
    public async Task<IActionResult> GetOrderStatusAsync(Guid id)
    {
        var orders = await _orderService.GetOrderStatusAsync(id);
        if(!orders.IsSuccess)
            return StatusCode(StatusCodes.Status500InternalServerError, orders.Errors);
        return Ok(orders.Value);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateOrderAsync([FromBody]CreateOrderDto orderDto)
    {
        var order = await  _orderService.CreateOrderAsync(orderDto);
        if (!order.IsSuccess)
            return StatusCode(StatusCodes.Status500InternalServerError, order.Errors);
        return Ok(order.Value);
    }
    
    [HttpPost("{id:guid}")]
    public async Task<IActionResult> CancelOrderAsync(Guid id)
    {
        var order = await _orderService.CancelOrderAsync(id);
        if (!order.IsSuccess)
            return StatusCode(StatusCodes.Status500InternalServerError, order.Errors);

        return Ok(order.IsSuccess);
    }
}