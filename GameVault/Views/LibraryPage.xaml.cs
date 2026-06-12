using System;
using System.Collections.Generic;
using System.Text;
using GameVault.ViewModels;
using GameVault.Models;
using GameVault.Services;

namespace GameVault.Views
{
    public partial class LibraryPage : ContentPage
    {
        private readonly LibraryViewModel viewModel;
        private readonly SelectedGameService selectedGameService;

        public LibraryPage(LibraryViewModel viewModel, SelectedGameService selectedGameService)
        {
            InitializeComponent();

            // FLOW:
            // LibraryPage receives LibraryViewModel from MauiProgram.
            // BindingContext connects LibraryPage.xaml labels/lists to LibraryViewModel.
            this.viewModel = viewModel;
            BindingContext = viewModel;
            this.selectedGameService = selectedGameService;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // FLOW:
            // Every time the Library page opens, reload saved games from SQLite.
            await viewModel.LoadLibraryAsync();
        }

        private async void GameCard_Tapped(object? sender, TappedEventArgs e)
        {
            if (sender is not Border border)
            {
                return;
            }

            if (border.BindingContext is not SavedGame selectedGame)
            {
                return;
            }

            selectedGameService.SelectedGame = selectedGame;
            await Shell.Current.GoToAsync(nameof(GameDetailsPage));
        }
    }
}
