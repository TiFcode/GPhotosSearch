using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GPhotosSearch.Tests.Dependencies
{
    public class FakeHttpMessageHandler : HttpMessageHandler
    {
        private readonly string _response;
        private readonly System.Net.HttpStatusCode _statusCode;

        public FakeHttpMessageHandler(string response, System.Net.HttpStatusCode statusCode = System.Net.HttpStatusCode.OK)
        {
            _response = response;
            _statusCode = statusCode;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new HttpResponseMessage
            {
                StatusCode = _statusCode,
                Content = new StringContent(_response, Encoding.UTF8, "application/json")
            });
        }
    }
}