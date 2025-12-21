namespace Friterie.API.DTOs;


using Friterie.Shared.Models;
using System.Collections.Generic;

public class CreateOrderDto
{
    public List<OrderItem> Items { get; set; } = new();
}