using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using GPhotosSearch.Dependencies.Authenticator;
using GPhotosSearch.Dependencies.HttpClientService;
using GPhotosSearch.Dependencies.Models;
using GPhotosSearch.Dependencies.UserInputOutput;
using GPhotosSearch.Processor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.UserSecrets;

public class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            IInputOutputHandler inputOutputHandler = new ConsoleInputOutputHandler();
            inputOutputHandler.WriteOutput("[START PROGRAM]");

            var configuration = new ConfigurationBuilder()
                .AddUserSecrets<Program>()
                .Build();

            var clientId = configuration["Google:ClientId"];
            var clientSecret = configuration["Google:ClientSecret"];
            if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientSecret))
            {
                inputOutputHandler.WriteOutput("Error: ClientId or ClientSecret is missing.");
                return;
            }

            inputOutputHandler.WriteOutput("Enter search text: ");
            string searchText = inputOutputHandler.ReadInput();

            inputOutputHandler.WriteOutput("[START AUTHENTICATION]");
            IAuthenticator authenticator = new GoogleAuthenticator(clientId, clientSecret);
            UserCredential credential = await authenticator.AuthenticateAsync();
            inputOutputHandler.WriteOutput("[END AUTHENTICATION]");

            inputOutputHandler.WriteOutput("[START SEARCH]");
            IHttpClientFactory httpClientFactory = new DefaultHttpClientFactory();
            var googlePhotosService =
                new GooglePhotosService(
                    credential,
                    httpClientFactory,
                    inputOutputHandler);
            var results = await googlePhotosService.SearchPhotosAsync(searchText);
            inputOutputHandler.WriteOutput("[END SEARCH]");

            inputOutputHandler.WriteOutput($"Found {results.Count} results:");
            inputOutputHandler.WriteOutput(results);
            inputOutputHandler.WriteOutput("[END PROGRAM]");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            Console.WriteLine("[END PROGRAM]");
        }
    }
}