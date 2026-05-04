using System;
using System.Collections.Generic;
using System.Text;

namespace GameVault.Models
{
    // FLOW:
    // GameDetailsPage creates a SavedGame when the user adds a game to the library.
    // SavedGameDatabaseService will store SavedGame objects in SQLite later.
    // LibraryViewModel will load them back and show them on the Profile / Library page.
    public class SavedGame : Game
    {
        // Status stores the user's progress for this game.
        public string Status { get; set; } = "Playing";

        // PersonalRating is the user's own score, separate from the API rating.
        public double PersonalRating { get; set; }

        // IsFavourite lets the library screen show favourite games later.
        public bool IsFavourite { get; set; }
    }
}
