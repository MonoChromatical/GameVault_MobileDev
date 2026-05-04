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

            if (StatusPicker.SelectedIndex < 0)
            {
                StatusPicker.SelectedIndex = 0;
            }

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

        private void AddToLibrary_Clicked(object? sender, EventArgs e)
        {
            if (SelectedGame == null)
            {
                SaveOutputLabel.Text = "No game selected.";
                return;
            }

            SavedGame savedGame = new SavedGame();
            savedGame.Title = SelectedGame.Title;
            savedGame.Genre = SelectedGame.Genre;
            savedGame.Platform = SelectedGame.Platform;
            savedGame.ReleaseDate = SelectedGame.ReleaseDate;
            savedGame.Rating = SelectedGame.Rating;
            savedGame.Description = SelectedGame.Description;
            savedGame.PersonalRating = RatingSlider.Value;

            if (StatusPicker.SelectedItem != null)
            {
                savedGame.Status = StatusPicker.SelectedItem.ToString() ?? "Playing";
            }

            SaveOutputLabel.Text = "Save screen is ready.";
        }

        private void RatingSlider_ValueChanged(object? sender, ValueChangedEventArgs e)
        {
            RatingOutputLabel.Text = $"Rating: {e.NewValue:0}/10";
        }
    }
}
