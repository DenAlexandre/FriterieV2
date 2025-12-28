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
    private readonly IOrderStore _orderStore;

    public OrderService(IOrderStore orderStore)
    {
        _orderStore = orderStore;
    }

    public async Task<int> CreateOrder(int userId)
    {
        //var order = new Order
        //{
        //    OrderUserId = userId,
        //    //Items = items,
        //    //OrderTotal = items.Sum(i => i.OiPrice * i.OiQuantity),
        //    OrderDatetime = DateTime.UtcNow,
        //    OrderStatus = (int)StatusTypeEnum.Créé,
        //};

        //// Réduire le stock
        //foreach (var item in items)
        //{



        //    //TODO
        //    //_orderService.UpdateOrderAsync(item.ProductId, item.Quantity);
        //}

        return await _orderStore.InsertOrderAsync(userId);
    }

    public Task<Order?> GetOrderById(int orderId) => _orderStore.GetByIdOrderAsync(orderId);

    public async Task<List<Order>> GetOrders(int userId) => await _orderStore.GetAllOrdersAsync(userId, 0 , 0);

    //public async Task<bool> UpdateOrderPaymentStatus(Orders order) =>
    //    await _orderStore.UpdateOrderAsync(order);
}