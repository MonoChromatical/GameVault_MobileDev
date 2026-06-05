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

        // These words are used by the adult content filter.

        private readonly List<string> adultFilterKeywords = new List<string>
        {
            "adult",
            "hentai",
            "sexual"
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
                    
                    if (ShouldShowGame(game) == true)
                    {
                        games.Add(game);
                    }
                }

                return games;
            }
            catch
            {
                return GetSampleGames();
            }
        }

        public async Task<List<Game>> SearchGamesAsync(string searchText, FilterOption? selectedGenre, FilterOption? selectedPlatform)
        {
            // FLOW:
            // DiscoverViewModel sends the user's search text, genre, and platform here.
            // RawgGameService builds a RAWG API URL.
            // RAWG returns matching games.
            // The JSON is converted into Game objects for the Discover page.

            if (RawgApiSettings.HasApiKey == false)
            {
                return GetSampleGames();
            }

            try
            {
                string url = RawgApiSettings.BaseUrl + "/games?key=" + RawgApiSettings.ApiKey + "&page_size=20";

                if (string.IsNullOrWhiteSpace(searchText) == false)
                {
                    // Escape the search text so spaces and special characters are safe in the URL.
                    string safeSearchText = Uri.EscapeDataString(searchText);
                    url = url + "&search=" + safeSearchText;
                }

                if (selectedGenre != null && string.IsNullOrWhiteSpace(selectedGenre.Value) == false)
                {
                    url = url + "&genres=" + selectedGenre.Value;
                }

                if (selectedPlatform != null && string.IsNullOrWhiteSpace(selectedPlatform.Value) == false)
                {
                    url = url + "&parent_platforms=" + selectedPlatform.Value;
                }

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

                    if (ShouldShowGame(game) == true)
                    {
                        games.Add(game);
                    }
                }

                return games;
            }
            catch
            {
                return GetSampleGames();
            }
        }

        public async Task<List<FilterOption>> GetGenreOptionsAsync()
        {
            // FLOW:
            // DiscoverViewModel asks for genres.
            // RawgGameService calls RAWG /genres.
            // RAWG returns names and slugs.
            // The app uses slugs in search URLs.

            if (RawgApiSettings.HasApiKey == false)
            {
                return GetSampleGenreOptions();
            }

            try
            {
                string url = RawgApiSettings.BaseUrl + "/genres?key=" + RawgApiSettings.ApiKey + "&page_size=40";
                string json = await httpClient.GetStringAsync(url);

                RawgFilterListResponse? response = JsonSerializer.Deserialize<RawgFilterListResponse>(json, jsonOptions);

                if (response == null || response.Results == null)
                {
                    return GetSampleGenreOptions();
                }

                List<FilterOption> options = new List<FilterOption>();

                FilterOption allOption = new FilterOption();
                allOption.Name = "All";
                allOption.Value = string.Empty;
                options.Add(allOption);

                foreach (RawgFilterItem item in response.Results)
                {
                    FilterOption option = new FilterOption();
                    option.Name = item.Name ?? "Unknown";
                    option.Value = item.Slug ?? string.Empty;

                    options.Add(option);
                }

                return options;
            }
            catch
            {
                return GetSampleGenreOptions();
            }
        }

        public async Task<List<FilterOption>> GetPlatformOptionsAsync()
        {
            // FLOW:
            // DiscoverViewModel asks for platform groups.
            // RawgGameService calls RAWG /platforms/lists/parents.
            // RAWG returns parent platform names and IDs.
            // The app uses IDs in search URLs.

            if (RawgApiSettings.HasApiKey == false)
            {
                return GetSamplePlatformOptions();
            }

            try
            {
                string url = RawgApiSettings.BaseUrl + "/platforms/lists/parents?key=" + RawgApiSettings.ApiKey;
                string json = await httpClient.GetStringAsync(url);

                RawgFilterListResponse? response = JsonSerializer.Deserialize<RawgFilterListResponse>(json, jsonOptions);

                if (response == null || response.Results == null)
                {
                    return GetSamplePlatformOptions();
                }

                List<FilterOption> options = new List<FilterOption>();

                FilterOption allOption = new FilterOption();
                allOption.Name = "All";
                allOption.Value = string.Empty;
                options.Add(allOption);

                foreach (RawgFilterItem item in response.Results)
                {
                    FilterOption option = new FilterOption();
                    option.Name = item.Name ?? "Unknown";
                    option.Value = item.Id.ToString();

                    options.Add(option);
                }

                return options;
            }
            catch
            {
                return GetSamplePlatformOptions();
            }
        }

        public async Task<string> GetGameDescriptionAsync(int gameId)
        {
            // FLOW:
            // GameDetailsPage sends the selected game's RAWG Id here.
            // RawgGameService calls RAWG /games/{id}.
            // RAWG returns description_raw.
            // GameDetailsPage displays that description.

            if (RawgApiSettings.HasApiKey == false)
            {
                return "Sample description is shown until a RAWG API key is added.";
            }

            try
            {
                string url = RawgApiSettings.BaseUrl + "/games/" + gameId + "?key=" + RawgApiSettings.ApiKey;
                string json = await httpClient.GetStringAsync(url);

                RawgGameDetailsResponse? response = JsonSerializer.Deserialize<RawgGameDetailsResponse>(json, jsonOptions);

                if (response == null || string.IsNullOrWhiteSpace(response.DescriptionRaw))
                {
                    return "No description available for this game.";
                }

                return response.DescriptionRaw;
            }
            catch
            {
                return "No description available for this game.";
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

        private bool ShouldShowGame(Game game)
        {
            if (UserSettings.ShowAdultContent == true)
            {
                return true;
            }

            string filterText = game.Title + " " + game.Genre;

            foreach (string keyword in adultFilterKeywords)
            {
                if (filterText.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }

            return true;
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

        private List<FilterOption> GetSampleGenreOptions()
        {
            // FLOW:
            // If RAWG cannot load genres, the app still shows basic picker options.
            // The second value is the RAWG genre slug used in search URLs.
            List<FilterOption> options = new List<FilterOption>();

            string[,] sampleGenres =
            {
                { "All", string.Empty },
                { "Action", "action" },
                { "RPG", "role-playing-games-rpg" },
                { "Adventure", "adventure" }
            };

            for (int index = 0; index < sampleGenres.GetLength(0); index++)
            {
                FilterOption option = new FilterOption();
                option.Name = sampleGenres[index, 0];
                option.Value = sampleGenres[index, 1];

                options.Add(option);
            }

            return options;
        }

        private List<FilterOption> GetSamplePlatformOptions()
        {
            // FLOW:
            // If RAWG cannot load platform groups, the app still shows basic picker options.
            // The second value is the RAWG parent_platforms id used in search URLs.
            List<FilterOption> options = new List<FilterOption>();

            string[,] samplePlatforms =
            {
                { "All", string.Empty },
                { "PC", "1" },
                { "PlayStation", "2" },
                { "Xbox", "3" },
                { "Nintendo", "7" }
            };

            for (int index = 0; index < samplePlatforms.GetLength(0); index++)
            {
                FilterOption option = new FilterOption();
                option.Name = samplePlatforms[index, 0];
                option.Value = samplePlatforms[index, 1];

                options.Add(option);
            }

            return options;
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

        private class RawgGameDetailsResponse
        {
            [JsonPropertyName("description_raw")]
            public string? DescriptionRaw { get; set; }
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

        private class RawgFilterListResponse
        {
            [JsonPropertyName("results")]
            public List<RawgFilterItem>? Results { get; set; }
        }

        private class RawgFilterItem
        {
            [JsonPropertyName("id")]
            public int Id { get; set; }

            [JsonPropertyName("name")]
            public string? Name { get; set; }

            [JsonPropertyName("slug")]
            public string? Slug { get; set; }
        }
    }
}
