 
using Friterie.API.Stores;
using Friterie.Shared.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Friterie.API.TestsUnits.Stores
{
    public class UserStoreTest 
    {

        private static readonly ILogger<UserStore> _logger = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();  // ou .AddDebug() ou rien du tout
        }).CreateLogger<UserStore>();

        private static readonly IConfiguration _config = new ConfigurationBuilder()
               .SetBasePath(Environment.CurrentDirectory)
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
               .AddEnvironmentVariables()
               .Build();

        private UserStore UserStore = new(_config, _logger);





        [Fact]
        public async Task DeleteUserAsync()
        {
            int user_id = 4;

            await UserStore.DeleteUserAsync(user_id);
        }


        [Fact]
        public async Task GetAllUsersAsync()
        {
            int limit = 100;
            int offset = 0;

            var list = await UserStore.GetAllUsersAsync(limit, offset);

            Assert.NotEqual(0, list.Count);
        }


        [Fact]
        public async Task GetByIdAsync()
        {
            int user_id = 5;
            var list = await UserStore.GetByIdAsync(user_id);

            Assert.Equal(user_id, list.UserId);
        }


        [Fact]
        public async Task InsertUserAsync()
        {

            //call friterie.ps_insert_users('den.alexandre@gmail.com', 'Denis', 'Alexandre', 'password', '0123456789', '"6 Rue de la clé, 45897 Moulinssard', now()::timestamp without time zone);

            User entity = new User
            {
                Email = "den.alexandre@gmail.com",
                FirstName = "Denis",
                LastName = "Alexandre",
                Password = "password",
                PhoneNumber = "0123456789",
                Address = "6 Rue de la clé, 45897 Moulinssard",
                Created = DateTime.UtcNow,

            };
            await UserStore.InsertUserAsync(entity);



        }

        [Fact]
        public async Task UpdateUserAsync()
        {



            User entity = new User
            {
                UserId = 5,
                Email = "den.alexandre2@gmail.com",
                FirstName = "Denis",
                LastName = "Alexandre",
                Password = "password",
                PhoneNumber = "0123456789",
                Address = "6 Rue de la clé, 45897 Moulinssard",
                Created = DateTime.UtcNow,

            };
            await UserStore.UpdateUserAsync(entity);
        }
    }
}