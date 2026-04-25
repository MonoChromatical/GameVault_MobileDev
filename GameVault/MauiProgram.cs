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

            builder.Services.AddTransient<HomeViewModel>();
            builder.Services.AddTransient<DiscoverViewModel>();

            builder.Services.AddSingleton<RawgGameService>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
