using Moq;
using Xunit;
using GPhotosSearch.Dependencies.HttpClientService;
using GPhotosSearch.Dependencies.Authenticator;
using GPhotosSearch.Dependencies;
using System.Threading.Tasks;
using System.Net.Http;
using Google.Apis.Http;
using GPhotosSearch.Dependencies.UserInputOutput;
using IHttpClientFactory = GPhotosSearch.Dependencies.HttpClientService.IHttpClientFactory;
using GPhotosSearch.Dependencies.Models;
using GPhotosSearch.Processor;
using Newtonsoft.Json;
using GPhotosSearch.Tests.Dependencies;

namespace GPhotosSearch.Tests
{
    public class GooglePhotosServiceTests
    {
        private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
        private readonly Mock<IInputOutputHandler> _inputOutputHandlerMock;
        private readonly Mock<GoogleAuthenticator> _googleAuthenticatorMock;

        public GooglePhotosServiceTests()
        {
            _httpClientFactoryMock = new Mock<IHttpClientFactory>();
            _inputOutputHandlerMock = new Mock<IInputOutputHandler>();
            _googleAuthenticatorMock = new Mock<GoogleAuthenticator>(null, null);
        }

        [Fact]
        public async Task SearchPhotosAsync_ThrowsHttpRequestException_WhenResponseIsError()
        {
            // Arrange
            string searchText = "flowers";
            var httpClient = new HttpClient(new FakeHttpMessageHandler("Error message", statusCode: System.Net.HttpStatusCode.BadRequest));

            _httpClientFactoryMock.Setup(f => f.Create()).Returns(httpClient);

            var service = new GooglePhotosService(_httpClientFactoryMock.Object, _inputOutputHandlerMock.Object, _googleAuthenticatorMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(() => service.SearchPhotosAsync(searchText));
        }

        [Fact]
        public async Task SearchPhotosAsync_ReturnsResults_WhenResponseIsValid()
        {
            // Arrange
            string searchText = "flowers";
            var mediaItems = new List<MediaItem>
            {
                new MediaItem { Id = "1", Filename = "photo1.jpg", MimeType = "image/jpeg" },
                new MediaItem { Id = "2", Filename = "photo2.jpg", MimeType = "image/jpeg" }
            };

            var searchResult = new SearchResult { MediaItems = mediaItems };

            var httpClient = new HttpClient(new FakeHttpMessageHandler(JsonConvert.SerializeObject(searchResult)));

            _httpClientFactoryMock.Setup(f => f.Create()).Returns(httpClient);

            var service = new GooglePhotosService(_httpClientFactoryMock.Object, _inputOutputHandlerMock.Object, _googleAuthenticatorMock.Object);

            // Act
            var results = await service.SearchPhotosAsync(searchText);

            // Assert
            Assert.Equal(mediaItems.Count, results.Count);
            Assert.Equal(mediaItems[0].Id, results[0].Id);
            Assert.Equal(mediaItems[1].Id, results[1].Id);
        }
    }
}
