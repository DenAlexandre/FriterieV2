namespace Friterie.API.Services;

 
using Friterie.API.Stores;
using Friterie.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Friterie.Shared.Models.EnumFriterie;

public class OrderService
{
    private readonly OrderStore _orderStore;

    public OrderService(OrderStore orderStore)
    {
        _orderStore = orderStore;
    }

    public async Task CreateOrder(int userId, List<OrderItem> items)
    {
        var order = new Orders
        {
            OrderUserId = userId,
            //Items = items,
            OrderTotal = items.Sum(i => i.OiPrice * i.OiQuantity),
            OrderDatetime = DateTime.UtcNow,
            OrderStatus = (int)StatusTypeEnum.Pending,
        };

        // Réduire le stock
        foreach (var item in items)
        {



            //TODO
            //_orderService.UpdateOrderAsync(item.ProductId, item.Quantity);
        }

        await _orderStore.InsertOrderAsync(order);
    }

    public Task<Orders?> GetOrderById(int orderId) => _orderStore.GetByIdOrderAsync(orderId);

    public async Task<List<Orders>> GetOrders(int userId) => await _orderStore.GetAllOrdersAsync(userId, 0 , 0);

    //public async Task<bool> UpdateOrderPaymentStatus(Orders order) =>
    //    await _orderStore.UpdateOrderAsync(order);
}