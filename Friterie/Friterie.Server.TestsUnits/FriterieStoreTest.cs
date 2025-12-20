using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Friterie.Models;
using Friterie.Server.Stores;


namespace Friterie.Server.TestsUnits
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

            aliments = await FriterieStore.GetAliments(0, 1000, 1000);

            Assert.NotNull(aliments);


        }
    }
}