using GameVault.Services;

namespace GameVault.Views
{
    public partial class SettingsPage : ContentPage
    {
        private readonly OrientationService orientationService;

        public SettingsPage(OrientationService orientationService)
        {
            // FLOW:
            // SettingsPage reads the saved adult content preference.
            // The switch displays the current saved value.
            InitializeComponent();

            this.orientationService = orientationService;

            AdultContentSwitch.IsToggled = UserSettings.ShowAdultContent;
            PortraitLockSwitch.IsToggled = UserSettings.LockRotation;

            UpdateSettingsMessage();
        }

        private void AdultContentSwitch_Toggled(object? sender, ToggledEventArgs e)
        {
            // FLOW:
            // User changes the switch.
            // UserSettings stores the value using MAUI Preferences.
            // RawgGameService reads this value when filtering results.
            UserSettings.ShowAdultContent = e.Value;

            UpdateSettingsMessage();
        }

        private void PortraitLockSwitch_Toggled(object? sender, ToggledEventArgs e)
        {
            UserSettings.LockRotation = e.Value;

            orientationService.ApplyPortraitLockSetting(e.Value);

            UpdateSettingsMessage();
        }

        private void UpdateSettingsMessage()
        {
            string adultFilterText;

            if (UserSettings.ShowAdultContent == true)
            {
                adultFilterText = "Adult content is shown";
            }
            else
            {
                adultFilterText = "Adult content is hidden";
            }

            string portraitLockText;

            if (UserSettings.LockRotation == true)
            {
                portraitLockText = "portrait view is locked on";
            }
            else
            {
                portraitLockText = "rotation follows the phone";
            }

            SettingsOutputLabel.Text = adultFilterText + ", " + portraitLockText + ".";
        }
    }
}


