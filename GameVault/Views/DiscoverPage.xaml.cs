using System.Collections.Generic;
using GameVault.Models;
using GameVault.Services;

namespace GameVault.Views
{
    public partial class DiscoverPage : ContentPage
    {
        private readonly RawgGameService rawgGameService;

        public DiscoverPage(RawgGameService rawgGameService)
        {
            InitializeComponent();
            this.rawgGameService = rawgGameService;
        }

        private async void TestSearch_Clicked(object? sender, EventArgs e)
        {
            List<Game> games = await rawgGameService.SearchGamesAsync(SearchBox.Text, null, null);

            SearchResultsList.ItemsSource = games;
            SearchResultLabel.Text = "Found " + games.Count + " game(s).";
        }
    }
}
