using System;
using System.Collections.Generic;
using System.Text;
using GameVault.Models;
using GameVault.Services;

namespace GameVault.Views
{
    public partial class GameDetailsPage : ContentPage
    {
        private readonly SelectedGameService selectedGameService;
        private readonly SavedGameDatabaseService savedGameDatabaseService;
        private readonly RawgGameService rawgGameService;

        public Game? SelectedGame
        {
            get
            {
                return selectedGameService.SelectedGame;
            }
        }

        public GameDetailsPage(SelectedGameService selectedGameService, SavedGameDatabaseService savedGameDatabaseService, RawgGameService rawgGameService)
        {
            InitializeComponent();

            // FLOW:
            // GameDetailsPage receives SelectedGameService and SavedGameDatabaseService from MauiProgram.
            // SelectedGameService gives this page the game the user tapped.
            // SavedGameDatabaseService saves the user's library entry to SQLite.
            this.selectedGameService = selectedGameService;
            this.savedGameDatabaseService = savedGameDatabaseService;
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

            // Refresh SelectedGame bindings each time the page appears.
            OnPropertyChanged(nameof(SelectedGame));
        }

        private async Task LoadDescriptionAsync()
        {
            // FLOW:
            // GameDetailsPage checks the selected Game.
            // If the description is missing or only a loading placeholder,
            // RawgGameService loads the full description from RAWG.
            // The SelectedGame binding is refreshed so XAML displays the new text.

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

        private async void AddToLibrary_Clicked(object? sender, EventArgs e)
        {
            if (SelectedGame == null)
            {
                SaveOutputLabel.Text = "No game selected.";
                return;
            }

            string status = "Playing";

            if (StatusPicker.SelectedItem != null)
            {
                status = StatusPicker.SelectedItem.ToString() ?? "Playing";
            }

            // Save a local copy of the selected game plus the user's library choices.
            SavedGame savedGame = new SavedGame();

            if (SelectedGame is SavedGame selectedSavedGame)
            {
                savedGame.ApiGameId = selectedSavedGame.ApiGameId;
            }
            else
            {
                savedGame.ApiGameId = SelectedGame.Id;
            }

            savedGame.Title = SelectedGame.Title;
            savedGame.Genre = SelectedGame.Genre;
            savedGame.Platform = SelectedGame.Platform;
            savedGame.ReleaseDate = SelectedGame.ReleaseDate;
            savedGame.Rating = SelectedGame.Rating;
            savedGame.Description = SelectedGame.Description;
            savedGame.Status = status;
            savedGame.PersonalRating = RatingSlider.Value;

            // FLOW:
            // The user's status, rating, and favourite choice are stored with the game.
            // SavedGameDatabaseService writes the object to SQLite.
            // LibraryPage reloads the database when the user opens the Library page.

            int rowsAdded = await savedGameDatabaseService.SaveGameAsync(savedGame);

            if (rowsAdded == 0)
            {
                SaveOutputLabel.Text = "Game is already in your library";
            }
            else
            {
                SaveOutputLabel.Text = "Game saved to library";
            }
        }

        private void RatingSlider_ValueChanged(object? sender, ValueChangedEventArgs e)
        {
            RatingOutputLabel.Text = $"Rating: {e.NewValue:0}/10";
        }
    }
}
