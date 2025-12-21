
using Friterie.API.Models;
using Friterie.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Friterie.API.Stores
{
    public interface IFriterieStore
    {

        Task<long> GetCountAliments();
        Task<List<Aliment>> GetAliments(int in_type, int in_limit, int in_offset);


        Task<Dictionary<int, Dictionary<int, List<GroupeAliment>>>> GetGroupesAliments();
        Task<List<Product>> GetProducts(int in_type, int in_limit, int in_offset);
    }




}
