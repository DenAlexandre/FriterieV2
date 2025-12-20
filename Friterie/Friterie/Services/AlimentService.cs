using Newtonsoft.Json;
using Friterie.Models;

namespace Friterie.Services
{
    public class AlimentService(IHttpClientFactory httpClientFactory, ILogger<AlimentService> logger) : IHostedService, IDisposable
    {

        const string Friterie_SERVICE_URI = "https://localhost:5001";
        private const string GET_COUNT_ALIMENTS_BDD = "/FriterieService/BDD/GetCountAliments";
        private const string GET_ALIMENTS_BDD = "/FriterieService/BDD/GetAliments";
        private const string GET_GROUPES_ALIMENTS_BDD = "/FriterieService/BDD/GetGroupesAliments";

        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
        private readonly ILogger _logger = logger;


        public async Task<long> GetCountAlimentsAsync()
        {
            long compteur = 0;

            try
            {
                // Construire l'URL avec le paramètre idRame
                var requestUri = $"{Friterie_SERVICE_URI}{GET_COUNT_ALIMENTS_BDD}";
                // Créer le client HTTP
                HttpClient client = _httpClientFactory.CreateClient();
                // Lire la réponse brute en tant que chaîne 
                var jsonResponse = await client.GetStringAsync(requestUri);
                // Désérialiser le JSON en un objet RameInformation
                var obj = JsonConvert.DeserializeObject<long>(jsonResponse);
                if (obj == null)
                {
                    Console.WriteLine("Deserialization resulted in null.");
                }
                compteur = obj;
            }
            catch (HttpRequestException httpRE)
            {
                if (httpRE.StatusCode == System.Net.HttpStatusCode.NotFound)
                    throw httpRE;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            return compteur;
        }




        public async Task<List<Aliment>> GetAlimentsAsync(int type, int limit, int offset)
        {
            List<Aliment> list = new List<Aliment>();

            try
            {
                // Construire l'URL avec le paramètre idRame
                var requestUri = $"{Friterie_SERVICE_URI}{GET_ALIMENTS_BDD}";
                requestUri += $"?in_type={Uri.EscapeDataString(type.ToString())}";
                requestUri += $"&in_limit={Uri.EscapeDataString(limit.ToString())}";
                requestUri += $"&in_offset={Uri.EscapeDataString(offset.ToString())}";
                // Créer le client HTTP
                HttpClient client = _httpClientFactory.CreateClient();
                // Lire la réponse brute en tant que chaîne 
                var jsonResponse = await client.GetStringAsync(requestUri);
                // Désérialiser le JSON en un objet RameInformation
                var obj = JsonConvert.DeserializeObject<List<Aliment>>(jsonResponse);
                if (obj == null)
                {
                    Console.WriteLine("Deserialization resulted in null.");
                }
                list = obj;
            }
            catch (HttpRequestException httpRE)
            {
                if (httpRE.StatusCode == System.Net.HttpStatusCode.NotFound)
                    throw httpRE;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            return list;
        }


        public async Task<Dictionary<int, Dictionary<int, List<GroupeAliment>>>> GetGroupesAlimentsAsync()
        {
            var dico = new Dictionary<int, Dictionary<int, List<GroupeAliment>>>();

            try
            {
                // Construire l'URL avec le paramètre idRame
                var requestUri = $"{Friterie_SERVICE_URI}{GET_GROUPES_ALIMENTS_BDD}";
                // Créer le client HTTP
                HttpClient client = _httpClientFactory.CreateClient();
                // Lire la réponse brute en tant que chaîne 
                var jsonResponse = await client.GetStringAsync(requestUri);
                // Désérialiser le JSON en un objet RameInformation
                var obj = JsonConvert.DeserializeObject<Dictionary<int, Dictionary<int, List<GroupeAliment>>>>(jsonResponse);
                if (obj == null)
                {
                    Console.WriteLine("Deserialization resulted in null.");
                }
                dico = obj;
            }
            catch (HttpRequestException httpRE)
            {
                if (httpRE.StatusCode == System.Net.HttpStatusCode.NotFound)
                    throw httpRE;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            return dico;
        }










        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Demarrage du AlimentService");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Dispose();
            _logger.LogInformation("Stop du AlimentService");
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _logger.LogInformation("Passage du Dispose du AlimentService");
        }
    }
}
