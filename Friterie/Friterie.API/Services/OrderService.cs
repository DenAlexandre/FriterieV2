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
        return await _orderStore.InsertOrderAsync(userId);
    }

    public Task<Order?> GetOrderById(int orderId) => _orderStore.GetByIdOrderAsync(orderId);

    public async Task<List<Order>> GetOrdersByUserId(int userId, int statusTypeEnum) => await _orderStore.GetOrdersByUserId(userId, statusTypeEnum);

    //public async Task<bool> UpdateOrderPaymentStatus(Orders order) =>
    //    await _orderStore.UpdateOrderAsync(order);
}