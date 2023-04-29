using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Google.Apis.Auth.OAuth2;
using GPhotosSearch.Dependencies;
using GPhotosSearch.Dependencies.HttpClientService;
using GPhotosSearch.Dependencies.Models;
using GPhotosSearch.Dependencies.UserInputOutput;

namespace GPhotosSearch.Processor
{
    public class GooglePhotosService
    {
        private readonly UserCredential _credential;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IInputOutputHandler _inputOutputHandler;

        public GooglePhotosService(
            UserCredential credential,
            IHttpClientFactory httpClientFactory,
            IInputOutputHandler inputOutputHandler)
        {
            _credential = credential;
            _httpClientFactory = httpClientFactory;
            _inputOutputHandler = inputOutputHandler;
        }

        public async Task<IList<MediaItem>> SearchPhotosAsync(string searchText)
        {
            try
            {
                using var httpClient = _httpClientFactory.Create();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _credential.Token.AccessToken);
                _inputOutputHandler.WriteOutput($"[INFO] _credential.Token.IssuedUtc == [{_credential.Token.IssuedUtc}]");
                _inputOutputHandler.WriteOutput($"[INFO] _credential.Token.ExpiresInSeconds == [{_credential.Token.ExpiresInSeconds}]");

                var requestUri = Constants.CONST_URL_GoogleAPI_Photos_Search;

                var requestBody = new
                {
                    pageSize = 100,
                    filters = new
                    {
                        contentFilter = new
                        {
                            includedContentCategories = new[] { searchText } // "ANIMALS", "CITYSCAPES", "LANDMARKS", "RECEIPTS", "SPORT", "WEDDINGS"
                        }
                    }
                };

                var requestContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(requestUri, requestContent);

                if (!response.IsSuccessStatusCode)
                {
                    _inputOutputHandler.WriteOutput($"Error: {response.StatusCode}");
                    return new List<MediaItem>();
                }

                var responseJson = await response.Content.ReadAsStringAsync();
                var responseDocument = JsonDocument.Parse(responseJson);

                var mediaItems = new List<MediaItem>();
                foreach (var item in responseDocument.RootElement.GetProperty("mediaItems").EnumerateArray())
                {
                    mediaItems.Add(new MediaItem
                    {
                        Id = item.GetProperty("id").GetString(),
                        Filename = item.GetProperty("filename").GetString(),
                        MimeType = item.GetProperty("mimeType").GetString()
                    });
                }

                return mediaItems;
            }
            catch (HttpRequestException ex)
            {
                _inputOutputHandler.WriteOutput($"Request exception: [{ex}]");
                return new List<MediaItem>();
            }
            catch (Exception ex)
            {
                _inputOutputHandler.WriteOutput($"An unexpected exception occurred: [{ex}]");
                return new List<MediaItem>();
            }
        }
    }
}
