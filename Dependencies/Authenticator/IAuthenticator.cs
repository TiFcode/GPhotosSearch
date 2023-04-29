using Google.Apis.Auth.OAuth2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPhotosSearch.Dependencies.Authenticator
{
    public interface IAuthenticator
    {
        Task<UserCredential> AuthenticateAsync();
    }
}
