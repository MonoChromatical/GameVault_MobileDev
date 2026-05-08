using GameVault.Models;
using GameVault.Services;
using System.Collections.ObjectModel;

namespace GameVault.ViewModels
{
    public class LibraryViewModel : BaseViewModel
    {
        private readonly SavedGameDatabaseService savedGameDatabaseService;
        private ObservableCollection<SavedGame> savedGames = new ObservableCollection<SavedGame>();

        public ObservableCollection<SavedGame> SavedGames
        {
            get
            {
                return savedGames;
            }
        }

        public LibraryViewModel(SavedGameDatabaseService savedGameDatabaseService)
        {
            this.savedGameDatabaseService = savedGameDatabaseService;
        }

        public async Task LoadLibraryAsync()
        {
            IsBusy = true;
            Message = "Loading library...";

            try
            {
                List<SavedGame> games = await savedGameDatabaseService.GetSavedGamesAsync();

                SavedGames.Clear();

                foreach (SavedGame game in games)
                {
                    SavedGames.Add(game);
                }

                Message = SavedGames.Count + " saved game(s).";
            }
            catch
            {
                Message = "Could not load saved games.";
            }

            IsBusy = false;
        }
    }
}