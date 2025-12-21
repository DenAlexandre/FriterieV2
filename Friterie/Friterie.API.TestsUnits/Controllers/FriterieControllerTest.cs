using Friterie.API.Models;
using Friterie.API.Stores;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;


using System.Collections.Generic;
using System.Reflection.Emit;


namespace Friterie.API.TestsUnits.Controllers
{
    public class FriterieControllerTest
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
        private readonly IConfiguration _configuration;
        private readonly IFriterieStore _FriterieStore;
        //https://localhost:5001/FriterieService/BDD/GetAliments
        private const string Friterie_SERVICE_URI = "https://localhost:5001";


        private const string GET_COUNT_ALIMENTS_BDD = "/FriterieService/BDD/GetCountAliments";
        private const string GET_ALIMENTS_BDD = "/FriterieService/BDD/GetAliments";
        private const string GET_GROUPES_ALIMENTS_BDD = "/FriterieService/BDD/GetGroupesAliments";

        private const string GET_PRODUCTS_BDD = "/FriterieService/BDD/GetProducts";

        [Fact]
        public async Task GetCountAlimentTest()
        {
            try
            {
                // Arrange
                HttpClient client = new HttpClient();
                var requestUri = $"{Friterie_SERVICE_URI}{GET_COUNT_ALIMENTS_BDD}";

                var jsonResponse = await client.GetStringAsync(requestUri);

                // Désérialiser la répoBnse JSON en dictionnaire
                var rep = JsonConvert.DeserializeObject<List<Aliment>>(jsonResponse);
                if (rep == null)
                    Console.WriteLine("Deserialization resulted in null.");

                // Assert
                Assert.Equal(rep.Count, 7);
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        [Fact]
        public async Task GetAlimentTest()
        {
            try
            {
                // Arrange
                HttpClient client = new HttpClient();
                // Lire la réponse brute en tant que chaîne
                // Construire l'URL avec le paramètre idRame
                //GetAlimentsBDD(int type, int limit, int offset)
                int type = 0;
                int limit = 1000;
                int offset = 1000;


                var requestUri = $"{Friterie_SERVICE_URI}{GET_ALIMENTS_BDD}";
                requestUri += $"?in_type={Uri.EscapeDataString(type.ToString())}";
                requestUri += $"&in_limit={Uri.EscapeDataString(limit.ToString())}";
                requestUri += $"&in_offset={Uri.EscapeDataString(offset.ToString())}";


                var jsonResponse = await client.GetStringAsync(requestUri);
 
                // Désérialiser la répoBnse JSON en dictionnaire
                var rep = JsonConvert.DeserializeObject<List<Aliment>>(jsonResponse);
                if (rep == null)
                    Console.WriteLine("Deserialization resulted in null.");

                // Assert
                Assert.Equal(rep.Count, 7);
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        [Fact]
        public async Task GetArticlesTest()
        {
            try
            {
                // Arrange
                HttpClient client = new HttpClient();
                // Lire la réponse brute en tant que chaîne
                // Construire l'URL avec le paramètre idRame
                //GetAlimentsBDD(int type, int limit, int offset)
                int type = 1;
                int limit = 1000;
                int offset = 0;


                var requestUri = $"{Friterie_SERVICE_URI}{GET_PRODUCTS_BDD}";
                requestUri += $"?in_type={Uri.EscapeDataString(type.ToString())}";
                requestUri += $"&in_limit={Uri.EscapeDataString(limit.ToString())}";
                requestUri += $"&in_offset={Uri.EscapeDataString(offset.ToString())}";


                var jsonResponse = await client.GetStringAsync(requestUri);

                // Désérialiser la répoBnse JSON en dictionnaire
                var rep = JsonConvert.DeserializeObject<List<Article>>(jsonResponse);
                if (rep == null)
                    Console.WriteLine("Deserialization resulted in null.");

                // Assert
                Assert.Equal(rep.Count, 2);
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}