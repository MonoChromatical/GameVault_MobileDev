using System;
using System.Collections.Generic;
using System.Text;
using GameVault.Models;
using SQLite;

namespace GameVault.Services
{
    // FLOW:
    // GameDetailsPage asks this service to save a game.
    // This service stores SavedGame objects in a local SQLite database.
    // LibraryViewModel asks this service to load saved games for the Library page.
    public class SavedGameDatabaseService
    {
        private SQLiteAsyncConnection? database;

        private async Task InitialiseAsync()
        {
            if (database != null)
            {
                return;
            }

            // FileSystem.AppDataDirectory is a safe app storage folder provided by MAUI.
            string databasePath = Path.Combine(FileSystem.AppDataDirectory, "gamevault.db3");

            database = new SQLiteAsyncConnection(databasePath);
            await database.CreateTableAsync<SavedGame>();
        }

        public async Task<List<SavedGame>> GetSavedGamesAsync()
        {
            // FLOW:
            // LibraryViewModel calls this method.
            // SQLite returns every saved game row.
            // LibraryViewModel then displays those rows and calculates totals.
            await InitialiseAsync();
            return await database!.Table<SavedGame>().ToListAsync();
        }

        public async Task<int> SaveGameAsync(SavedGame savedGame)
        {
            // FLOW:
            // GameDetailsPage creates a SavedGame from the selected Game.
            // SQLite inserts that SavedGame into the local database.
            // LibraryPage reloads the saved rows when it appears.
            await InitialiseAsync();
            return await database!.InsertAsync(savedGame);
        }
    }
}
