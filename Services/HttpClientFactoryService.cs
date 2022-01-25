using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using WarCardGame.DTO;
using WarCardGame.Entities;

namespace WarCardGame.Services
{
    public class HttpClientFactoryService : IHttpClientServiceImplementation
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly WarGameApiClient _warGameApiClient;
        private readonly JsonSerializerOptions _options;

        public HttpClientFactoryService(IHttpClientFactory httpClientFactory, WarGameApiClient warGameApiClient)
        {
            _httpClientFactory = httpClientFactory;
            this._warGameApiClient = warGameApiClient;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<List<string>> StartNewGame()
        {
            return await StartNewGameClient();
        }

        public async Task<List<Player>> GetAllPlayers()
        {
            return await GetAllPlayersClient();
        }

        public async Task<Player> GetPlayerById(int id)
        {
            return await GetPlayerByIdClient(id);
        }

        public async Task InsertGameResults(Match match)
        {
            await InsertGameResultsClient(match);
        }

        public async Task UpdatePlayerResults(PlayersResultsDTO player)
        {
            await UpdatePlayersResultsClient(player);
        }

        private async Task<List<string>> StartNewGameClient() => await _warGameApiClient.StartNewGame();

        private async Task<List<Player>> GetAllPlayersClient() => await _warGameApiClient.GetAllPlayers();

        private async Task<Player> GetPlayerByIdClient(int id) => await _warGameApiClient.GetPlayerById(id);

        private async Task InsertGameResultsClient(Match match) => await _warGameApiClient.InsertMatchResults(match);

        private async Task UpdatePlayersResultsClient(PlayersResultsDTO player)
                                    => await _warGameApiClient.UpdatePlayerResults(player);
        /*
        private async Task<List<Player>> GetCompaniesWithHttpClientFactory()
        {
            var httpClient = _httpClientFactory.CreateClient("WarGameApiClient");
            using (var response = await httpClient.GetAsync("Players/GetAllPlayers", HttpCompletionOption.ResponseHeadersRead))
            {
                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<List<Player>>(stream, _options);
            }
        }

        private async Task<Player> GetPlayerByIdClientFactory(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("WarGameApiClient");
            using (var response = await httpClient.GetAsync("Players/GetPlayerById/" + id, HttpCompletionOption.ResponseHeadersRead))
            {
                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<Player>(stream, _options);
            }
        }*/
    }
}
