using System.Collections.ObjectModel;
using GameVault.Models;
namespace GameVault.ViewModels
{
    public class DiscoverViewModel : BaseViewModel
    {
        public string SearchText { get; set; } = string.Empty;
        public string Message { get; set; } = "Search for a game.";
        public ObservableCollection<Game> FilteredGames { get; set; } = new ObservableCollection<Game>();
    }
}