namespace GameVault.Services
{
    // FLOW:
    // SettingsPage changes these values.
    // RawgGameService reads these values.
    // The app uses them to decide which games should be displayed.
    public static class UserSettings
    {
        private const string ShowAdultContentKey = "ShowAdultContent";
        private const string LockRotationKey = "LockRotation";

        public static bool ShowAdultContent
        {
            get
            {
                return Preferences.Get(ShowAdultContentKey, false);
            }
            set
            {
                Preferences.Set(ShowAdultContentKey, value);
            }
        }
        public static bool LockRotation
        {
            get
            {
                return Preferences.Get(LockRotationKey, false);
            }
            set
            {
                Preferences.Set(LockRotationKey, value);
            }
        }
    }
}
