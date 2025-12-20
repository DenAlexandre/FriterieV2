using Nutrition.Authentication;
using Newtonsoft.Json;

namespace Nutrition.Services
{
    public class UserServiceView(IHttpClientFactory httpClientFactory, Serilog.ILogger logger) : IHostedService, IDisposable
    {
        const string USER_SERVICE_URI = "http://localhost:45588";
        private const string GET_USER_BY_LOGIN = "/UserService/BDD/GetUserByLogin";

        private readonly Serilog.ILogger _logger = logger;
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
        //private List<UserAccount> _users;
        private UserSDR? _userSDR;

        //UserAccount? means that the method GetByUserName can return a valid UserAccount object or null. 
        //public UserAccount? GetByUserName(string userName)
        //{
        //    return _users.FirstOrDefault(x => x.UserName == userName);
        //}


        /// <summary>
        /// Retourne l'utilisateur demandé
        /// </summary>
        /// <returns></returns>
        public async Task<UserSDR?> GetUserByLogin(string login)
        {
            try
            {

                // Construire l'URL avec le paramètre idRame
                var requestUri = $"{USER_SERVICE_URI}{GET_USER_BY_LOGIN}?login={login}";
                // Créer le client HTTP

                // Effectuer une requête GET

                var jsonResponse = await _httpClientFactory.CreateClient().GetStringAsync(requestUri);
                // Désérialiser le JSON en un objet NettoyageInformation
                _userSDR = JsonConvert.DeserializeObject<UserSDR>(jsonResponse);
                if (_userSDR == null)
                    _logger.Warning("Le login :" + login + " n'existe pas dans la base de donnée !");

                return _userSDR;

            }
            catch (HttpRequestException httpEx)
            {
                throw httpEx;
            }
            catch (Exception ex)
            {
                throw;
            }

        }





        public async Task StartAsync(CancellationToken cancellationToken)
        {
            //_userSDR = await GetUserByLogin("dalexandre");
            //return  Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {

        }
    }
}
