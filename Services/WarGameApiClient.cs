using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WarCardGame.DTO;
//using Newtonsoft.Json;
using WarCardGame.Entities;

namespace WarCardGame.Services
{
    public class WarGameApiClient
    {
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _options;

        public WarGameApiClient(HttpClient client)
        {
            this._client = client;

            _client.BaseAddress = new Uri("https://localhost:5001/api/");
            _client.Timeout = new TimeSpan(0, 0, 30);
            _client.DefaultRequestHeaders.Clear();

            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<List<string>> StartNewGame()
        {
            using (var response = await _client.GetAsync("Matches/StartNewGame", HttpCompletionOption.ResponseHeadersRead))
            {
                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<List<string>>(stream, _options);
            }
        }

        public async Task<List<Player>> GetAllPlayers()
        {
            using (var response = await _client.GetAsync("Players/GetAllPlayers", HttpCompletionOption.ResponseHeadersRead))
            {
                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<List<Player>>(stream, _options);
            }
        }

        public async Task<Player> GetPlayerById(int id)
        {
            using (var response = await _client.GetAsync("Players/GetPlayerById/" + id, HttpCompletionOption.ResponseHeadersRead))
            {
                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<Player>(stream, _options);
            }
        }

        public async Task InsertMatchResults(Match matchResult)
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(matchResult);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            using (var response = await _client.PostAsync("Matches/AddMatch", data))
            {
                string result = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine("Match Results Inserted.");
                response.EnsureSuccessStatusCode();
                //var stream = await response.Content.ReadAsStreamAsync();
                //return await JsonSerializer.DeserializeAsync<Player>(stream, _options);
            }
        }

        public async Task UpdatePlayerResults(PlayersResultsDTO player)
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(player);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            using (var response = await _client.PutAsync("Players/UpdatePlayers", data))
            {
                string result = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine($"PlayerId {player.Id} Statistics --- Updated.");
                response.EnsureSuccessStatusCode();
                //var stream = await response.Content.ReadAsStreamAsync();
                //return await JsonSerializer.DeserializeAsync<Player>(stream, _options);
            }
        }
    }
}
