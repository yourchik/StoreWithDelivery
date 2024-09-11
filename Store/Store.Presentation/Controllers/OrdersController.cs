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

    [HttpGet("GetOrdersByFilter")]
    public async Task<IActionResult> GetOrdersByFilter(BaseFilter<Order> filter, int page, int pageSize)
    {
        var orders = await _orderService.GetOrdersByFilterAsync(filter, page, pageSize);
        if(!orders.IsSuccess)
            return StatusCode(StatusCodes.Status500InternalServerError, orders.Errors);
        return Ok(orders.Value);
    }
    

    [HttpGet("GetOrder/{id}")]
    public async Task<IActionResult> GetOrder(Guid id)
    {
        var order = await _orderService.GetOrderAsync(id);
        if (!order.IsSuccess)
            return StatusCode(StatusCodes.Status500InternalServerError, order.Errors);
        return Ok(order.Value);
        
    }
    
    [HttpGet("GetOrderStatus/{id}")]
    public async Task<IActionResult> GetOrderStatus(Guid id)
    {
        var orders = await _orderService.GetOrderStatusAsync(id);
        if(!orders.IsSuccess)
            return StatusCode(StatusCodes.Status500InternalServerError, orders.Errors);
        return Ok(orders.Value);
    }
    
    [HttpPost("CreateOrder")]
    public async Task<IActionResult> CreateOrder([FromBody]CreateOrderDto orderDto)
    {
        var order = await  _orderService.CreateOrderAsync(orderDto);
        if (!order.IsSuccess)
            return StatusCode(StatusCodes.Status500InternalServerError, order.Errors);
        return Ok(order.Value);
    }
    
    [HttpPost("CancelOrder/{id}")]
    public async Task<IActionResult> CancelOrder(Guid id)
    {
        var order = await _orderService.CancelOrderAsync(id);
        if (!order.IsSuccess)
            return StatusCode(StatusCodes.Status500InternalServerError, order.Errors);

        return Ok(order.IsSuccess);
    }
}