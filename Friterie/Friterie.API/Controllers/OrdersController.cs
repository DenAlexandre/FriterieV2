namespace Friterie.API.Controllers;

using Friterie.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;







[Authorize]
[ApiController]
public class OrdersController : ControllerBase
{

    private const string REMOVE_PRODUCT_IN_ORDER = "FriterieAPI/api/orders/remove-product";
    private const string ADD_PRODUCT_IN_ORDER = "FriterieAPI/api/orders/add-product";
    private const string ADD_ITEMS_IN_ORDER = "FriterieAPI/api/add-items-in-order";
    private const string ADD_ORDER = "FriterieAPI/api/add-order";
    private const string GET_ORDER_BY_USER_ID = "FriterieAPI/api/get-order-by-user-id";


    private readonly OrderService _orderService;

    public OrdersController(OrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost(ADD_ORDER)]
    public IActionResult CreateOrder([FromBody] int UserID)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null)
            return Unauthorized();

        var userId = int.Parse(userIdClaim);
        var orderId = _orderService.CreateOrder(userId);

        return Ok(orderId);
    }

    //[HttpGet("{id}")]
    //public IActionResult GetOrder(int id)
    //{
    //    var order = _orderService.GetOrderById(id);
    //    if (order == null)
    //        return NotFound();

    //    return Ok(order);
    //}

    [HttpGet(GET_ORDER_BY_USER_ID)]
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