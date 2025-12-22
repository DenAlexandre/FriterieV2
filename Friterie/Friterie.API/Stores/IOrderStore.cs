
 
using Friterie.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Friterie.API.Stores
{
    public interface IOrderStore
    {
        public  Task<Orders?> GetByIdOrderAsync(int order_id);

        public  Task<List<Orders>> GetAllOrdersAsync(int userid, int limit, int offset);


        public  Task InsertOrderAsync(Orders entity);


        public  Task<bool> UpdateOrderAsync(Orders entity);



        public Task DeleteOrderAsync(int order_id);








        public  Task<OrderItem?> GetOrderItemByIdAsync(int oi_id);


        //public  Task<List<OrderItem>> GetAllOrderItemAsync(int limit, int offset);

        public  Task InsertOrderItemAsync(OrderItem entity);


        public  Task UpdateOrderItemAsync(OrderItem entity);


        public Task DeleteOrderItemAsync(int oi_id);

    }




}
