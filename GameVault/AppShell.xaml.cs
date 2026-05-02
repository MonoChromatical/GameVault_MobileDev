using GameVault.Views;

namespace GameVault;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // FLOW:
        // HomePage and DiscoverPage navigate to GameDetailsPage by route name.
        // Registering the route here lets Shell create GameDetailsPage when needed.
        Routing.RegisterRoute(nameof(GameDetailsPage), typeof(GameDetailsPage));
    }
}
