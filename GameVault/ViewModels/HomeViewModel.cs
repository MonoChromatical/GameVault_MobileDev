using System;
using System.Collections.Generic;
using System.Text;
using GameVault.Models;
using GameVault.Services;
using System.Collections.ObjectModel;

namespace GameVault.ViewModels
{
    // FLOW:
    // HomePage.xaml binds to HomeViewModel.
    // HomeViewModel asks RawgGameService for games.
    // RawgGameService returns Game objects from RAWG or sample fallback data.
    // HomeViewModel stores those Game objects.
    // HomePage.xaml displays them through Binding.
    public class HomeViewModel : BaseViewModel
    {
        private readonly RawgGameService rawgGameService;

        private Game featuredGame = new Game();

        private ObservableCollection<Game> popularGames = new ObservableCollection<Game>();

        public Game FeaturedGame
        {
            get
            {
                return featuredGame;
            }
            set
            {
                featuredGame = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Game> PopularGames
        {
            get
            {
                return popularGames;
            }
        }

        public HomeViewModel(RawgGameService rawgGameService)
        {
            this.rawgGameService = rawgGameService;

            // The constructor starts the first load.
            LoadHomeGamesAsync();
        }

        private async Task LoadHomeGamesAsync()
        {
            IsBusy = true;
            Message = "Loading games...";

            try
            {
                List<Game> games = await rawgGameService.GetPopularGamesAsync();

                PopularGames.Clear();

                foreach (Game game in games)
                {
                    PopularGames.Add(game);
                }

                if (PopularGames.Count > 0)
                {
                    FeaturedGame = PopularGames[0];
                    Message = "Games loaded.";
                }
                else
                {
                    Message = "No games found.";
                }
            }
            catch
            {
                Message = "Could not load games.";
            }

            IsBusy = false;
        }
    }
}