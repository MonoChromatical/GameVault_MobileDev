using GameVault.Models;

namespace GameVault.Services
{
    public class RawgGameService
    {
        public Task<List<Game>> GetPopularGamesAsync()
        {
            List<Game> games = new List<Game>();
            games.Add(new Game { Id = 1, Title = "Sample API Game", Genre = "Action", Platform = "PC", Rating = 4.5 });
            return Task.FromResult(games);
        }
    }
}
