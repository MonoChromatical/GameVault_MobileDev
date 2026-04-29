namespace GameVault.Models
{
    // FLOW:
    // RawgGameService creates FilterOption objects from RAWG genre/platform data.
    // DiscoverViewModel stores these options.
    // DiscoverPage.xaml displays Name to the user.
    // RawgGameService uses Value when searching RAWG.
    public class FilterOption
    {
        public string Name { get; set; } = string.Empty;

        public string Value { get; set; } = string.Empty;
    }
}
