namespace Friterie.API.Controllers;

using Friterie.API.DTOs;
using Friterie.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using static Friterie.Shared.Models.EnumFriterie;

//[Authorize]
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
    public async Task<IActionResult> CreateOrder([FromBody] int userId)
    {
        if (userId <= 0)
            return BadRequest("UserId invalide");

        var orderId = _orderService.CreateOrder(userId);
        return Ok(new { orderId });
    }




    [HttpGet(GET_ORDER_BY_USER_ID)]
    public async Task<IActionResult> GetOrdersByUserId([FromBody] int userId, int statusTypeEnum)
    {
        var orders = _orderService.GetOrdersByUserId(userId, statusTypeEnum);
        if (orders == null)
            return NotFound();

        return Ok(orders);
    }

}