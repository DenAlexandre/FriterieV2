
using Friterie.API.Models;
using Friterie.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Friterie.API.Stores
{
    public interface IOrderStore
    {
        public  Task<Orders?> GetByIdOrderAsync(int order_id);

        public  Task<List<Orders>> GetAllOrdersAsync(int limit, int offset);


        public  Task InsertOrderAsync(Orders entity);


        public  Task UpdateOrderAsync(Orders entity);



        public Task DeleteOrderAsync(int order_id);





    }




}
