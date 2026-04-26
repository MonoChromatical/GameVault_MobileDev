using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using GameVault.Models;

namespace GameVault.Services
{
    // FLOW:
    // ViewModel asks RawgGameService for games.
    // RawgGameService calls RAWG API and receives JSON.
    // JSON is converted into Game objects.
    // ViewModel stores those Game objects for XAML to display.
    public class RawgGameService
    {
        private readonly HttpClient httpClient = new HttpClient();


        private readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
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

            try
            {
                int currentYear = DateTime.Now.Year;
                string dateRange = currentYear + "-01-01," + currentYear + "-12-31";
                // FLOW:
                // RAWG returns games from the current year.
                // ordering=-added makes the list closer to RAWG's popular/best-of-year view.
                string url = RawgApiSettings.BaseUrl + "/games?key=" + RawgApiSettings.ApiKey + "&page_size=10&dates=" + dateRange + "&ordering=-added";
                string json = await httpClient.GetStringAsync(url);

                RawgGameListResponse? response = JsonSerializer.Deserialize<RawgGameListResponse>(json, jsonOptions);

                if (response == null || response.Results == null)
                {
                    return GetSampleGames();
                }

                List<Game> games = new List<Game>();

                foreach (RawgGame rawgGame in response.Results)
                {
                    Game game = ConvertRawgGame(rawgGame);
                    games.Add(game);
                }

                return games;
            }
            catch
            {
                return GetSampleGames();
            }
        }

        private Game ConvertRawgGame(RawgGame rawgGame)
        {
            Game game = new Game();
            game.Id = rawgGame.Id;
            game.Title = rawgGame.Name ?? "Unknown Game";
            game.Genre = GetGenreText(rawgGame);
            game.Platform = GetPlatformText(rawgGame);
            game.ReleaseDate = rawgGame.Released ?? "Unknown";
            game.Rating = rawgGame.Rating;
            // The game list does not include the full description, so the Details page loads it later.
            game.Description = "Loading description...";

            return game;
        }
        private string GetGenreText(RawgGame rawgGame)
        {
            if (rawgGame.Genres == null || rawgGame.Genres.Count == 0)
            {
                return "Unknown";
            }

            List<string> genreNames = new List<string>();

            foreach (RawgNamedItem genre in rawgGame.Genres)
            {
                if (string.IsNullOrWhiteSpace(genre.Name) == false)
                {
                    genreNames.Add(genre.Name);
                }
            }

            return string.Join(", ", genreNames);
        }

        private string GetPlatformText(RawgGame rawgGame)
        {
            if (rawgGame.Platforms == null || rawgGame.Platforms.Count == 0)
            {
                return "Unknown";
            }

            List<string> platformNames = new List<string>();

            foreach (RawgPlatformInfo platformInfo in rawgGame.Platforms)
            {
                if (platformInfo.Platform != null && string.IsNullOrWhiteSpace(platformInfo.Platform.Name) == false)
                {
                    platformNames.Add(platformInfo.Platform.Name);
                }
            }

            return string.Join(", ", platformNames);
        }

        private List<Game> GetSampleGames()
        {
            List<Game> games = new List<Game>();

            Game firstGame = new Game();
            firstGame.Id = 1;
            firstGame.Title = "Sample API Game";
            firstGame.Genre = "Action RPG";
            firstGame.Platform = "PC / Console";
            firstGame.ReleaseDate = "2026";
            firstGame.Rating = 4.5;
            firstGame.Description = "Sample data is shown until a RAWG API key is added.";

            games.Add(firstGame);

            return games;
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

            [JsonPropertyName("released")]
            public string? Released { get; set; }

            [JsonPropertyName("rating")]
            public double Rating { get; set; }

            [JsonPropertyName("genres")]
            public List<RawgNamedItem>? Genres { get; set; }

            [JsonPropertyName("platforms")]
            public List<RawgPlatformInfo>? Platforms { get; set; }
        }

        private class RawgNamedItem
        {
            [JsonPropertyName("name")]
            public string? Name { get; set; }
        }

        private class RawgPlatformInfo
        {
            [JsonPropertyName("platform")]
            public RawgNamedItem? Platform { get; set; }
        }

    }
}
