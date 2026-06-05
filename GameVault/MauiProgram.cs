using GameVault.Views;
using Microsoft.Extensions.Logging;
using GameVault.ViewModels;
using GameVault.Services;

namespace GameVault
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddTransient<HomePage>();
            builder.Services.AddTransient<DiscoverPage>();
            builder.Services.AddTransient<GameDetailsPage>();
            builder.Services.AddTransient<LibraryPage>();
            builder.Services.AddTransient<SettingsPage>();
            builder.Services.AddTransient<AboutPage>();

            // SelectedGameService temporarily stores the game the user tapped.
            // FLOW:
            // HomePage/DiscoverPage -> SelectedGameService -> GameDetailsPage.
            builder.Services.AddSingleton<SelectedGameService>();

            // SavedGameDatabaseService stores and loads the user's library games.
            // FLOW:
            // GameDetailsPage -> SavedGameDatabaseService -> SQLite database.
            builder.Services.AddSingleton<SavedGameDatabaseService>();

            // OrientationService controls whether the phone is locked to portrait or landscape.
            // FLOW:
            // SettingsPage -> OrientationService -> Android screen orientation.
            builder.Services.AddSingleton<OrientationService>();

            // HomeViewModel is registered so MAUI can give it to HomePage automatically.
            // FLOW:
            // MauiProgram -> creates HomeViewModel -> injects it into HomePage.
            builder.Services.AddTransient<HomeViewModel>();

            // DiscoverViewModel is used by DiscoverPage for search input and results.
            // FLOW:
            // MauiProgram -> creates DiscoverViewModel -> injects it into DiscoverPage.
            builder.Services.AddTransient<DiscoverViewModel>();

            // LibraryViewModel loads saved games and calculates library stats.
            // FLOW:
            // MauiProgram -> creates LibraryViewModel -> injects it into LibraryPage.
            builder.Services.AddTransient<LibraryViewModel>();

            // RawgGameService is registered here so future ViewModels can request API data.
            // FLOW:
            // MauiProgram -> creates RawgGameService -> injects it into ViewModels.
            builder.Services.AddSingleton<RawgGameService>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
