using Microsoft.AspNetCore.Mvc;
using Store.Application.Services.Interfaces.Entities;
using Store.Domain.Entities;

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

    [HttpGet]
    public async Task<OkObjectResult> GetOrders()
    {
        var orders = await _orderService.GetOrdersAsync();
        return Ok(orders);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrder(int id)
    {
        var order = await  _orderService.GetOrderAsync(id);
        if (order == null)
        {
            return NotFound();
        }
        return Ok(order);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody]Order order)
    {
        await  _orderService.CreateOrderAsync(order);
        return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
    }
    
    [HttpDelete("{id}")]
    public async Task<NoContentResult> CancelOrder(int id)
    {
        await  _orderService.CancelOrderAsync(id);
        return NoContent();
    }
}