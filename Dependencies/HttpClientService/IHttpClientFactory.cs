using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPhotosSearch.Dependencies.HttpClientService
{
    public interface IHttpClientFactory
    {
        HttpClient Create();
    }
}
