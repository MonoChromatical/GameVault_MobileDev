namespace GameVault;

// ==============================
// APPLICATION STARTUP
// ==============================

// FLOW:
// App is created when GameVault starts.
// App creates AppShell, which contains the app navigation menu.
public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        // The Window hosts AppShell so the user can move between Home,
        // Discover, Library, Settings, and About.
        return new Window(new AppShell());
    }
}
