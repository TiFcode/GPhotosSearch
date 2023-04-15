using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPhotosSearch.Dependencies.HttpClientService
{
    internal class DefaultHttpClientFactory : IHttpClientFactory
    {
        public HttpClient Create() => new HttpClient();
    }
}
