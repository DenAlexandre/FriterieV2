
 
using Friterie.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Friterie.API.Stores
{
    public interface IProductStore
    {

        public Task<Product> GetProductById(int id);
        public Task<List<Product>> GetProducts(int in_type, int in_limit, int in_offset);
    }




}
