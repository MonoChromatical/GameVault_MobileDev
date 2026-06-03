namespace GameVault.Models
{
    // FLOW:
    // RAWG API data will be converted into Game objects.
    // ViewModels will store these Game objects.
    // XAML pages will bind to the ViewModel properties and display the game data.
    public class Game
    {
        // Id identifies the game inside the app or from the API.
        public int Id { get; set; }

        // Basic game information displayed on Home, Discover, and Details screens.
        public string Title { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public string Platform { get; set; } = string.Empty;
        public string ReleaseDate { get; set; } = string.Empty;
        public double Rating { get; set; }

        // Extra details used when the user opens the Game Details page.
        public string Description { get; set; } = string.Empty;
    }
}
