using System.Collections.ObjectModel;
using GameVault.Models;
namespace GameVault.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        public ObservableCollection<Game> PopularGames { get; set; } = new ObservableCollection<Game>();
        public HomeViewModel()
        {
            PopularGames.Add(new Game { Title = "Sample Game", Genre = "Action", Platform = "PC" });
            PopularGames.Add(new Game { Title = "Another Game", Genre = "Adventure", Platform = "Console" });
        }
    }
}