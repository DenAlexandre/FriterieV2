 
using Friterie.API.Stores;
using Friterie.Shared.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;




namespace Friterie.API.TestsUnits.Stores
{
    public class FriterieStoreTest
    {

        private static readonly ILogger<FriterieStore> _logger = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();  // ou .AddDebug() ou rien du tout
        }).CreateLogger<FriterieStore>();

        private static readonly IConfiguration _config = new ConfigurationBuilder()
               .SetBasePath(Environment.CurrentDirectory)
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
               .AddEnvironmentVariables()
               .Build();

        private FriterieStore FriterieStore = new(_config, _logger);


        [Fact]
        public async Task GetCountAlimentsTest()
        {
            long compteur = 0;

            compteur = await FriterieStore.GetCountAliments();

            Assert.NotEqual(0,compteur);


        }



        [Fact]
        public async Task GetAlimentsTest()
        {
            List<Aliment> aliments = new List<Aliment>();

            aliments = await FriterieStore.GetAliments(0, 1000, 0);

            Assert.NotNull(aliments);


        }


        [Fact]
        public async Task GetArticlesTest()
        {
            List<Product> products = new List<Product>();

            products = await FriterieStore.GetProducts(1, 1000, 0);

            Assert.NotNull(products);


        }
    }
}