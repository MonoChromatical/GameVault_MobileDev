namespace GameVault.Services
{
    public static class UserSettings
    {
        private const string ShowAdultContentKey = "ShowAdultContent";
        public static bool ShowAdultContent
        {
            get { return Preferences.Get(ShowAdultContentKey, false); }
            set { Preferences.Set(ShowAdultContentKey, value); }
        }
    }
}