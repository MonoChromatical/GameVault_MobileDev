using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace GameVault.Models
{
    // FLOW:
    // GameDetailsPage will create a SavedGame when the user adds a game to the library.
    // SavedGameDatabaseService will store SavedGame objects in SQLite.
    // LibraryViewModel will load them back and show them on the Profile / Library page.
    public class SavedGame : Game
    {
        // SQLite uses this as the local database row Id, separate from the RAWG API Id.
        [PrimaryKey, AutoIncrement]
        public new int Id { get; set; }

        // Status stores the user's progress for this game.
        public string Status { get; set; } = "Playing";

        // PersonalRating is the user's own score, separate from the API rating.
        public double PersonalRating { get; set; }

        // IsFavourite lets the library screen show favourite games later.
        public bool IsFavourite { get; set; }
    }
}
