using GameVault.Models;
using GameVault.Services;
using System.Collections.ObjectModel;


namespace GameVault.ViewModels
{
    // FLOW:
    // DiscoverPage.xaml binds to DiscoverViewModel.
    // The user types into the SearchBar and chooses filter options.
    // DiscoverPage.xaml.cs calls RunSearchAsync when Search is pressed.
    // DiscoverViewModel asks RawgGameService for fresh RAWG API results.
    // FilteredGames is displayed by the CollectionView.
    public class DiscoverViewModel : BaseViewModel
    {
        private readonly RawgGameService rawgGameService;

        private string searchText = string.Empty;

        private ObservableCollection<Game> filteredGames = new ObservableCollection<Game>();

        public ObservableCollection<FilterOption> GenreOptions { get; } = new ObservableCollection<FilterOption>();

        public ObservableCollection<FilterOption> PlatformOptions { get; } = new ObservableCollection<FilterOption>();

        public FilterOption? SelectedGenre { get; set; }

        public FilterOption? SelectedPlatform { get; set; }

        public string SearchText
        {
            get
            {
                return searchText;
            }
            set
            {
                searchText = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Game> FilteredGames
        {
            get
            {
                return filteredGames;
            }
        }

        public DiscoverViewModel(RawgGameService rawgGameService)
        {
            this.rawgGameService = rawgGameService;

            // Load filter options and initial search results when the DiscoverViewModel is created.
            LoadPageAsync();
        }

        private async Task LoadPageAsync()
        {
            await LoadFilterOptionsAsync();
            await RunSearchAsync();
        }

        private async Task LoadFilterOptionsAsync()
        {
            // FLOW:
            // DiscoverViewModel loads filter options from RAWG.
            // The options are shown in the Pickers.
            // The selected option is passed back to RawgGameService during search.

            List<FilterOption> genres = await rawgGameService.GetGenreOptionsAsync();

            GenreOptions.Clear();

            foreach (FilterOption genre in genres)
            {
                GenreOptions.Add(genre);
            }

            if (GenreOptions.Count > 0)
            {
                SelectedGenre = GenreOptions[0];
            }

            List<FilterOption> platforms = await rawgGameService.GetPlatformOptionsAsync();

            PlatformOptions.Clear();

            foreach (FilterOption platform in platforms)
            {
                PlatformOptions.Add(platform);
            }

            if (PlatformOptions.Count > 0)
            {
                SelectedPlatform = PlatformOptions[0];
            }

            OnPropertyChanged(nameof(SelectedGenre));
            OnPropertyChanged(nameof(SelectedPlatform));
        }

        public async Task RunSearchAsync()
        {
            // FLOW:
            // DiscoverPage.xaml.cs calls this when the Search button is pressed.
            // This asks RawgGameService for fresh API results.
            // FilteredGames is then updated for the CollectionView.

            IsBusy = true;
            Message = "Searching RAWG...";

            FilteredGames.Clear();

            List<Game> searchResults = await rawgGameService.SearchGamesAsync(SearchText, SelectedGenre, SelectedPlatform);

            foreach (Game game in searchResults)
            {
                FilteredGames.Add(game);
            }

            Message = FilteredGames.Count + " result(s) found.";
            IsBusy = false;
        }
    }
}