using System.Collections.Generic;
using GameVault.Models;
using GameVault.Services;
using GameVault.ViewModels;

namespace GameVault.Views
{
    public partial class HomePage : ContentPage
    {
        private readonly HomeViewModel viewModel;
        private readonly RawgGameService rawgGameService;

        public HomePage(HomeViewModel viewModel, RawgGameService rawgGameService)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            this.rawgGameService = rawgGameService;
            BindingContext = viewModel;
        }

        private async void TestApiLoad_Clicked(object? sender, EventArgs e)
        {
            List<Game> games = await rawgGameService.GetPopularGamesAsync();

            viewModel.PopularGames.Clear();

            foreach (Game game in games)
            {
                viewModel.PopularGames.Add(game);
            }
        }
    }
}
