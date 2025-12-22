 
using Friterie.API.Stores;
using Friterie.Shared.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;




namespace Friterie.API.TestsUnits.Stores
{
    public class ProductStoreTest
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

        private ProductStore _productStore = new(_config, _logger);



        [Fact]
        public async Task GetProductsTest()
        {
            List<Product> products = new List<Product>();

            products = await _productStore.GetProducts(1, 1000, 0);

            Assert.NotNull(products);


        }
    }
}