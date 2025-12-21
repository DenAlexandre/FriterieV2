namespace Friterie.API.Services;

using Friterie.API.Models;
using Friterie.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;

public class OrderService
{
    private readonly DataService _dataService;

    public OrderService(DataService dataService)
    {
        _dataService = dataService;
    }

    public Order CreateOrder(int userId, List<OrderItem> items)
    {
        var order = new Order
        {
            UserId = userId,
            Items = items,
            TotalAmount = items.Sum(i => i.Price * i.Quantity),
            OrderDate = DateTime.UtcNow,
            Status = "Pending"
        };

        // Réduire le stock
        foreach (var item in items)
        {
            _dataService.UpdateProductStock(item.ProductId, item.Quantity);
        }

        return _dataService.AddOrder(order);
    }

    public Order? GetOrderById(int orderId) => _dataService.GetOrderById(orderId);

    public List<Order> GetUserOrders(int userId) => _dataService.GetUserOrders(userId);

    public bool UpdateOrderPaymentStatus(int orderId, bool isPaid, string paymentIntentId) =>
        _dataService.UpdateOrderPaymentStatus(orderId, isPaid, paymentIntentId);
}