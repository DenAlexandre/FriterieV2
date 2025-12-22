 
using Friterie.API.Stores;
using Friterie.Shared.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;


using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection.Emit;


namespace Friterie.API.TestsUnits.Controllers
{
    public class ProductControllerTest
    {

        private static readonly ILogger<ProductStore> _logger = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();  // ou .AddDebug() ou rien du tout
        }).CreateLogger<ProductStore>();

        private static readonly IConfiguration _config = new ConfigurationBuilder()
               .SetBasePath(Environment.CurrentDirectory)
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
               .AddEnvironmentVariables()
               .Build();



        private const string FRITERIE_SERVICE_URI = "https://localhost:5001";


        private const string GET_PRODUCTS_BDD = "/FriterieAPI/api/products/GetProducts";
        private const string GET_PRODUCTS_BY_CATEGORY_BDD = "/FriterieAPI/api/products/category";
        private const string GET_PRODUCTS_BY_ID_BDD = "/FriterieAPI/api/products/";



        [Fact]
        public async Task GetProducts_ReturnsProducts_ForValidType()
        {
            using var client = new HttpClient();

            int type = 1;
            int limit = 1000;
            int offset = 0;

            var requestUri =
                $"{FRITERIE_SERVICE_URI}{GET_PRODUCTS_BDD}" +
                $"?type={type}&limit={limit}&offset={offset}";

            var jsonResponse = await client.GetStringAsync(requestUri);

            var products = JsonConvert.DeserializeObject<List<Product>>(jsonResponse);

            Assert.NotNull(products);
            Assert.NotEmpty(products);
        }
    }
}