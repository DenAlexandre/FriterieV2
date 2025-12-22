namespace Friterie.API.DTOs;

public class CreatePaymentIntentDto
{
    public decimal Amount { get; set; }
}

public class ConfirmPaymentDto
{
    public int OrderId { get; set; }
    public string PaymentIntentId { get; set; } = string.Empty;
}