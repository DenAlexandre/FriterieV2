namespace Friterie.API.Controllers;

using Friterie.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly OrderService _orderService;

    public OrdersController(OrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost]
    public IActionResult CreateOrder([FromBody] CreateOrderDto dto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null)
            return Unauthorized();

        var userId = int.Parse(userIdClaim);
        var order = _orderService.CreateOrder(userId, dto.Items);

        return Ok(order);
    }

    [HttpGet("{id}")]
    public IActionResult GetOrder(int id)
    {
        var order = _orderService.GetOrderById(id);
        if (order == null)
            return NotFound();

        return Ok(order);
    }

    [HttpGet("user")]
    public IActionResult GetUserOrders()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null)
            return Unauthorized();

        var userId = int.Parse(userIdClaim);
        var orders = _orderService.GetOrders(userId);

        return Ok(orders);
    }
}