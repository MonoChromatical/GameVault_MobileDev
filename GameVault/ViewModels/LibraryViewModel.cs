using System;
using System.Collections.Generic;
using System.Text;
using GameVault.Models;
using GameVault.Services;
using System.Collections.ObjectModel;

namespace GameVault.ViewModels
{
    // FLOW:
    // LibraryPage.xaml binds to LibraryViewModel.
    // LibraryViewModel asks SavedGameDatabaseService for saved games.
    // SavedGameDatabaseService loads SavedGame objects from SQLite.
    // LibraryViewModel calculates totals.
    // LibraryPage.xaml displays the totals and saved games.
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

        public int TotalGames
        {
            get
            {
                return SavedGames.Count;
            }
        }

        public int PlayingCount
        {
            get
            {
                return CountByStatus("Playing");
            }
        }

        public int CompletedCount
        {
            get
            {
                return CountByStatus("Completed");
            }
        }

        public int WishlistCount
        {
            get
            {
                return CountByStatus("Wishlist");
            }
        }

        public int DroppedCount
        {
            get
            {
                return CountByStatus("Dropped");
            }
        }

        public double AverageRating
        {
            get
            {
                if (SavedGames.Count == 0)
                {
                    return 0;
                }

                double total = 0;

                foreach (SavedGame savedGame in SavedGames)
                {
                    total = total + savedGame.PersonalRating;
                }

                return total / SavedGames.Count;
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

                RefreshStats();

                Message = TotalGames + " saved game(s).";
            }
            catch
            {
                Message = "Could not load saved games.";
            }

            IsBusy = false;
        }

        private int CountByStatus(string status)
        {
            // FLOW:
            // Library statistics call this helper once for each status.
            // It counts matching SavedGame.Status values from the SQLite-loaded list.
            int count = 0;

            foreach (SavedGame savedGame in SavedGames)
            {
                if (savedGame.Status == status)
                {
                    count++;
                }
            }

            return count;
        }

        private void RefreshStats()
        {
            // FLOW:
            // SavedGames changes after SQLite loads.
            // These notifications tell the XAML labels to recalculate their bindings.
            OnPropertyChanged(nameof(TotalGames));
            OnPropertyChanged(nameof(PlayingCount));
            OnPropertyChanged(nameof(CompletedCount));
            OnPropertyChanged(nameof(WishlistCount));
            OnPropertyChanged(nameof(DroppedCount));
            OnPropertyChanged(nameof(AverageRating));
        }
    }
}
