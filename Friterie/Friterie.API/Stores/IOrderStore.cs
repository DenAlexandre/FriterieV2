
 
using Friterie.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Friterie.API.Stores
{
    public interface IOrderStore
    {
        public  Task<Order?> GetByIdOrderAsync(int order_id);

        public  Task<List<Order>> GetAllOrdersAsync(int userid, int limit, int offset);


        public  Task<int> InsertOrderAsync(int userId);


        public  Task<bool> UpdateOrderAsync(Order entity);



        public Task DeleteOrderAsync(int order_id);








        public  Task<OrderItem?> GetOrderItemByIdAsync(int oi_id);


        //public  Task<List<OrderItem>> GetAllOrderItemAsync(int limit, int offset);

        public  Task InsertOrderItemAsync(OrderItem entity);


        public  Task UpdateOrderItemAsync(OrderItem entity);


        public Task DeleteOrderItemAsync(int oi_id);



        //Task<List<Order>> GetOrdersByUserId(int userId, Stat v1, int v2);
        Task<List<Order>> GetOrdersByUserId(int userId, int statusTypeEnum);
    }




}
