using System;
using GameVault.ViewModels;

namespace GameVault.Views
{
    public partial class HomePage : ContentPage
    {
        public HomePage(HomeViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = viewModel;
        }

        private async void BrowseGames_Clicked(object? sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//discover");
        }
    }
}
