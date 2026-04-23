namespace GameVault.Models
{
    public class Game
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public string Platform { get; set; } = string.Empty;
        public string ReleaseDate { get; set; } = string.Empty;
        public double Rating { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}