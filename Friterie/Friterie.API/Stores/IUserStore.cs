
 
using Friterie.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Friterie.API.Stores
{
    public interface IUserStore
    {
        public Task<User?> GetByIdAsync(int user_id);

        public Task<List<User>> GetAllUsersAsync(int limit, int offset);

        public Task InsertUserAsync(User entity);


        public Task UpdateUserAsync(User entity);


        public Task DeleteUserAsync(int user_id);


    }




}
