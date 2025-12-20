using Friterie.Models;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Friterie.Server.Stores
{
    public interface IFriterieStore
    {

        Task<long> GetCountAliments();
        Task<List<Aliment>> GetAliments(int in_type, int in_limit, int in_offset);


        Task<Dictionary<int, Dictionary<int, List<GroupeAliment>>>> GetGroupesAliments();

    }




}
