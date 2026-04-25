namespace GameVault.Services
{
    public static class RawgApiSettings
    {
        public const string BaseUrl = "https://api.rawg.io/api";
        public const string ApiKey = " ";
        public static bool HasApiKey
        {
            get
            {
                return string.IsNullOrWhiteSpace(ApiKey) == false;
            }
        }
    }
}