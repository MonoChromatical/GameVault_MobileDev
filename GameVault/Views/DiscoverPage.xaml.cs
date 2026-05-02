using System;
using System.Collections.Generic;
using System.Text;
using GameVault.Models;
using GameVault.Services;
using GameVault.ViewModels;

namespace GameVault.Views
{
    public partial class DiscoverPage : ContentPage
    {
        private readonly DiscoverViewModel viewModel;
        private readonly SelectedGameService selectedGameService;

        public DiscoverPage(DiscoverViewModel viewModel, SelectedGameService selectedGameService)
        {
            InitializeComponent();

            // FLOW:
            // DiscoverPage receives DiscoverViewModel and SelectedGameService from MauiProgram.
            // BindingContext connects DiscoverPage.xaml controls to ViewModel properties.
            this.viewModel = viewModel;
            this.selectedGameService = selectedGameService;
            BindingContext = viewModel;
        }

        private async void Search_Clicked(object? sender, EventArgs e)
        {
            // FLOW:
            // User taps Search.
            // DiscoverPage sends the event to DiscoverViewModel.
            // DiscoverViewModel asks RawgGameService for fresh RAWG data.
            await viewModel.RunSearchAsync();
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
            // Tapped Discover card has a Game as its BindingContext.
            // Store that Game in SelectedGameService.
            // Then navigate to GameDetailsPage.
            selectedGameService.SelectedGame = selectedGame;
            await Shell.Current.GoToAsync(nameof(GameDetailsPage));
        }
    }
}
