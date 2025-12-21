namespace Friterie.API.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Friterie.API.Services;
using Friterie.API.DTOs;
using System.Threading.Tasks;
using System;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly PaymentService _paymentService;
    private readonly OrderService _orderService;

    public PaymentController(PaymentService paymentService, OrderService orderService)
    {
        _paymentService = paymentService;
        _orderService = orderService;
    }

    [HttpPost("create-intent")]
    public async Task<IActionResult> CreatePaymentIntent([FromBody] CreatePaymentIntentDto dto)
    {
        try
        {
            var (clientSecret, paymentIntentId) = await _paymentService.CreatePaymentIntent(dto.Amount);

            return Ok(new
            {
                clientSecret,
                paymentIntentId
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost("confirm")]
    public async Task<IActionResult> ConfirmPayment([FromBody] ConfirmPaymentDto dto)
    {
        try
        {
            var isConfirmed = await _paymentService.ConfirmPayment(dto.PaymentIntentId);

            if (isConfirmed)
            {
                _orderService.UpdateOrderPaymentStatus(dto.OrderId, true, dto.PaymentIntentId);
                return Ok(new { success = true, message = "Paiement confirmé avec succès" });
            }

            return BadRequest(new { success = false, message = "Échec du paiement" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}