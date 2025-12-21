using Friterie.API.Models;
using Friterie.API.Stores;
using Friterie.Shared.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

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
            int user_id;

            var list = await UserStore.GetAllUsersAsync(0, 100);
        }

        [Fact]
        public async Task GetAllAsync()
        {


            var list = await UserStore.GetAllUsersAsync(0,100);

            Assert.NotEqual(0, list.Count);


        }


        [Fact]
        public async Task GetAllUsersAsync()
        {
            int limit;
            int offset;
        }


        [Fact]
        public async Task GetByIdAsync()
        {
            int user_id;
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
                Email = ""
            };
        }
    }
}