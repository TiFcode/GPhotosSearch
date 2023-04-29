using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GPhotosSearch.Dependencies.Authenticator
{
    public class GoogleAuthenticator : IAuthenticator
    {
        private readonly string _clientId;
        private readonly string _clientSecret;

        public GoogleAuthenticator(
            string clientId,
            string clientSecret)
        {
            _clientId = clientId;
            _clientSecret = clientSecret;
        }

        public async Task<UserCredential> AuthenticateAsync()
        {
            var secrets = new ClientSecrets
            {
                ClientId = _clientId,
                ClientSecret = _clientSecret
            };
            var memoryDataStore = new InMemoryDataStore();
            var initializer = new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = secrets,
                Scopes = new[] { Constants.CONST_URL_GoogleAPI_Auth_Photos_ReadOnly },
                DataStore = memoryDataStore,
            };

            var codeReceiver = new LocalServerCodeReceiver();

            var flow = new GoogleAuthorizationCodeFlow(initializer);
            var _credential = await new AuthorizationCodeInstalledApp(flow, codeReceiver).AuthorizeAsync("user", CancellationToken.None);

            if (_credential.Token.IsExpired(_credential.Flow.Clock))
            {
                await _credential.RefreshTokenAsync(CancellationToken.None);
            }
            return _credential;
        }
    }
}
