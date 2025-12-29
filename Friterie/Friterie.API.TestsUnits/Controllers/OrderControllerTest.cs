 
using Friterie.API.Stores;
using Friterie.Shared.Models;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;


using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection.Emit;


namespace Friterie.API.TestsUnits.Controllers
{
    public class OrderControllerTest
    {
        private const string ADD_ORDER = "FriterieAPI/api/add-order";
        private const string GET_ORDER_BY_USER_ID = "FriterieAPI/api/get-order-by-user-id";



        private static readonly ILogger<OrderStore> _logger = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();  // ou .AddDebug() ou rien du tout
        }).CreateLogger<OrderStore>();

        private static readonly IConfiguration _config = new ConfigurationBuilder()
               .SetBasePath(Environment.CurrentDirectory)
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
               .AddEnvironmentVariables()
               .Build();



        private const string FRITERIE_SERVICE_URI = "https://localhost:5001";




        [Fact]
        public async Task GetOrderByUserID()
        {
            using var client = new HttpClient();

            var query = new Dictionary<string, string?>
            {
                ["p_user_id"] = "8",
                ["p_status_id"] = "0"
            };
            var requestUri = QueryHelpers.AddQueryString(FRITERIE_SERVICE_URI + GET_ORDER_BY_USER_ID, query);

            var orders = await client.GetFromJsonAsync<List<Order>>(requestUri);

            Assert.NotNull(orders);
            Assert.NotEmpty(orders);
        }















    }
}