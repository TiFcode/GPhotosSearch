using Google.Apis.Auth.OAuth2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPhotosSearch.Dependencies.Authenticator
{
    internal class GoogleAuthenticator : IAuthenticator
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
            return await GoogleWebAuthorizationBroker.AuthorizeAsync(
                secrets,
                new[] { Dependencies.Constants.CONST_URL_GoogleAPI_Auth_Photos_ReadOnly },
                "user",
                CancellationToken.None);
        }
    }
}
