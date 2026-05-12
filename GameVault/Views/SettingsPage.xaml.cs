using GameVault.Services;
namespace GameVault.Views
{
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage() { InitializeComponent(); AdultContentSwitch.IsToggled = UserSettings.ShowAdultContent; UpdateSettingsMessage(); }
        private void AdultContentSwitch_Toggled(object? sender, ToggledEventArgs e) { UserSettings.ShowAdultContent = e.Value; UpdateSettingsMessage(); }
        private void UpdateSettingsMessage() { SettingsOutputLabel.Text = UserSettings.ShowAdultContent ? "Adult content is shown." : "Adult content is hidden."; }
    }
}