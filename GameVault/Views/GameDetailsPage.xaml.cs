using System;
using GameVault.Models;
using GameVault.Services;

namespace GameVault.Views
{
    public partial class GameDetailsPage : ContentPage
    {
        private readonly SelectedGameService selectedGameService;
        private readonly RawgGameService rawgGameService;

        public Game? SelectedGame
        {
            get
            {
                return selectedGameService.SelectedGame;
            }
        }

        public GameDetailsPage(SelectedGameService selectedGameService, RawgGameService rawgGameService)
        {
            InitializeComponent();

            this.selectedGameService = selectedGameService;
            this.rawgGameService = rawgGameService;

            BindingContext = this;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await LoadDescriptionAsync();
            OnPropertyChanged(nameof(SelectedGame));
        }

        private async Task LoadDescriptionAsync()
        {
            if (SelectedGame == null)
            {
                return;
            }

            if (SelectedGame.Description != "Loading description..." && string.IsNullOrWhiteSpace(SelectedGame.Description) == false)
            {
                return;
            }

            SelectedGame.Description = await rawgGameService.GetGameDescriptionAsync(SelectedGame.Id);
            OnPropertyChanged(nameof(SelectedGame));
        }
    }
}
