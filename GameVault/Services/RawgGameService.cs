using System.Text.Json;
using System.Text.Json.Serialization;
using GameVault.Models;

namespace GameVault.Services
{
    public class RawgGameService
    {
        private readonly HttpClient httpClient = new HttpClient();

        private readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        public RawgGameService()
        {
            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("GameVault-Ver1");
        }

        public async Task<List<Game>> GetPopularGamesAsync()
        {
            if (RawgApiSettings.HasApiKey == false)
            {
                return GetSampleGames();
            }

            int currentYear = DateTime.Now.Year;
            string dateRange = currentYear + "-01-01," + currentYear + "-12-31";
            string url = RawgApiSettings.BaseUrl + "/games?key=" + RawgApiSettings.ApiKey + "&page_size=10&dates=" + dateRange + "&ordering=-added";
            string json = await httpClient.GetStringAsync(url);
            RawgGameListResponse? response = JsonSerializer.Deserialize<RawgGameListResponse>(json, jsonOptions);

            List<Game> games = new List<Game>();
            if (response != null && response.Results != null)
            {
                foreach (RawgGame rawgGame in response.Results)
                {
                    games.Add(new Game { Id = rawgGame.Id, Title = rawgGame.Name ?? "Unknown Game", Rating = rawgGame.Rating });
                }
            }
            return games;
        }

        private List<Game> GetSampleGames()
        {
            return new List<Game> { new Game { Id = 1, Title = "Sample API Game", Genre = "Action RPG", Platform = "PC / Console", Rating = 4.5 } };
        }

        private class RawgGameListResponse
        {
            [JsonPropertyName("results")]
            public List<RawgGame>? Results { get; set; }
        }

        private class RawgGame
        {
            [JsonPropertyName("id")]
            public int Id { get; set; }
            [JsonPropertyName("name")]
            public string? Name { get; set; }
            [JsonPropertyName("rating")]
            public double Rating { get; set; }
        }
    }
}
