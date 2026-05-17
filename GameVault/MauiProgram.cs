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

            builder.Services.AddSingleton<SelectedGameService>();
            builder.Services.AddSingleton<SavedGameDatabaseService>();
            builder.Services.AddSingleton<OrientationService>();

            builder.Services.AddTransient<HomeViewModel>();
            builder.Services.AddTransient<DiscoverViewModel>();
            builder.Services.AddTransient<LibraryViewModel>();

            builder.Services.AddSingleton<RawgGameService>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}