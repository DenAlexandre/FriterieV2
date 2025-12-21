using Friterie.API.Models;
using Friterie.API.Stores;
using Friterie.Shared.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Friterie.API.TestsUnits.Stores
{
    public class OrderStoreTest 
    {

        private static readonly ILogger<OrderStore> _logger = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();  // ou .AddDebug() ou rien du tout
        }).CreateLogger<OrderStore>();

        private static readonly IConfiguration _config = new ConfigurationBuilder()
               .SetBasePath(Environment.CurrentDirectory)
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
               .AddEnvironmentVariables()
               .Build();

        private OrderStore OrderStore = new(_config, _logger);





        [Fact]
        public async Task DeleteOrderAsync()
        {
            int user_id = 1;

            await OrderStore.DeleteOrderAsync(user_id);
        }


        [Fact]
        public async Task GetAllOrdersAsync()
        {
            int limit = 100;
            int offset = 0;

            var list = await OrderStore.GetAllOrdersAsync(limit, offset);

            Assert.NotEqual(0, list.Count);
        }


        [Fact]
        public async Task GetByIdOrderAsync()
        {
            int user_id = 2;
            var list = await OrderStore.GetByIdOrderAsync(user_id);

            Assert.Equal(user_id, list.OrderId);
        }


        [Fact]
        public async Task InsertOrderAsync()
        {
            //call friterie.sp_insert_orders(7,now()::timestamp without time zone, 25.50, 2,'',true);
            Orders entity = new Orders
            {
                //OrderId = 5,
                OrderUserId = 4,
                OrderDatetime = DateTime.UtcNow,
                OrderTotal = 25.50m,
                OrderStatus = 2,
                OrderIntentId = "",
                OrderIsPaid = true,
            };
            await OrderStore.InsertOrderAsync(entity);



        }

        [Fact]
        public async Task UpdateOrderAsync()
        {



            Orders entity = new Orders
            {
               OrderId = 2,
                OrderUserId = 4,
                OrderDatetime = DateTime.UtcNow,
                OrderTotal = 25.50m,
                OrderStatus = 2,
                OrderIntentId = "",
                OrderIsPaid = true,
            };
            await OrderStore.UpdateOrderAsync(entity);
        }
    }
}