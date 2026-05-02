using System;
using System.Collections.Generic;
using System.Text;
using GameVault.Models;
using GameVault.Services;
using GameVault.ViewModels;

namespace GameVault.Views
{
    public partial class HomePage : ContentPage
    {
        private readonly HomeViewModel viewModel;
        private readonly SelectedGameService selectedGameService;

        public HomePage(HomeViewModel viewModel, SelectedGameService selectedGameService)
        {
            InitializeComponent();

            // FLOW:
            // HomePage receives HomeViewModel and SelectedGameService from MauiProgram.
            // BindingContext connects HomePage.xaml bindings to HomeViewModel properties.
            this.viewModel = viewModel;
            this.selectedGameService = selectedGameService;
            BindingContext = viewModel;
        }

        private async void BrowseGames_Clicked(object? sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//discover");
        }

        private async void FeaturedGame_Clicked(object? sender, EventArgs e)
        {
            selectedGameService.SelectedGame = viewModel.FeaturedGame;
            await Shell.Current.GoToAsync(nameof(GameDetailsPage));
        }

        private async void GameCard_Tapped(object? sender, TappedEventArgs e)
        {
            if (sender is not Border border)
            {
                return;
            }

            if (border.BindingContext is not Game selectedGame)
            {
                return;
            }

            // FLOW:
            // Tapped card has a Game as its BindingContext.
            // Store that Game in SelectedGameService.
            // Then navigate to GameDetailsPage.
            selectedGameService.SelectedGame = selectedGame;
            await Shell.Current.GoToAsync(nameof(GameDetailsPage));
        }
    }
}
