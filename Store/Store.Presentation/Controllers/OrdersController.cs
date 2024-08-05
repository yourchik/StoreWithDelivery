using Microsoft.AspNetCore.Mvc;
using Store.Application.Dtos.Order;
using Store.Application.Services.Interfaces.Entities;

namespace Store.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet("GetOrders")]
    public async Task<IActionResult> GetOrders()
    {
        var orders = await _orderService.GetOrdersAsync();
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

    [HttpPost("CreateOrder")]
    public async Task<IActionResult> CreateOrder([FromBody]CreateOrderDto orderDto)
    {
        var order = await  _orderService.CreateOrderAsync(orderDto);
        if (!order.IsSuccess)
            return StatusCode(StatusCodes.Status500InternalServerError, order.Errors);
        return Ok(order.Value);
    }
    
    [HttpDelete("CancelOrder/{id}")]
    public async Task<IActionResult> CancelOrder(Guid id)
    {
        var order = await _orderService.CancelOrderAsync(id);
        if (!order.IsSuccess)
            return StatusCode(StatusCodes.Status500InternalServerError, order.Errors);

        return Ok(order.IsSuccess);
    }
}