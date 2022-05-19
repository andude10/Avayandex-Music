using Avayandex_Music.Core.Services;
using Avayandex_Music.Presentation.ViewModels.Controls;
using Avayandex_Music.Presentation.Views.Controls;
using DynamicData;
using Yandex.Music.Api;
using Yandex.Music.Api.Models.Album;
using Yandex.Music.Api.Models.Artist;
using Yandex.Music.Api.Models.Playlist;
using Yandex.Music.Api.Models.Track;

namespace Avayandex_Music.Presentation.ViewModels;

public class SearchResultViewModel : ViewModelBase, IRoutableViewModel
{
#region Fields

    private SearchBarViewModel _searchBarViewModel;
    private CardsViewModel<YArtist> _artistsCardsViewModel;
    private CardsViewModel<YPlaylist> _playlistsCardsView;
    private CardsViewModel<YAlbum> _albumsCardsViewModel;
    private ListViewModel<YTrack> _podcastEpisodesViewModel;
    private ListViewModel<YTrack> _tracksViewModel;

#endregion

    public SearchResultViewModel(IScreen screen, string searchText)
    {
        HostScreen = screen;
        SearchBarViewModel = new SearchBarViewModel(screen)
        {
            SearchText = searchText
        };

        SearchCommand = ReactiveCommand.CreateFromTask(SearchAsync);
        
        InitializeViewModels();
    }
    
#region Commands

    /// <summary>
    ///      Searching for data on the entered text
    /// </summary>
    public ReactiveCommand<Unit, Unit> SearchCommand { get; }

#endregion

#region Properties

    public SearchBarViewModel SearchBarViewModel
    {
        get => _searchBarViewModel;
        set => this.RaiseAndSetIfChanged(ref _searchBarViewModel, value);
    }
    
    public CardsViewModel<YArtist> ArtistsCardsViewModel
    {
        get => _artistsCardsViewModel;
        set => this.RaiseAndSetIfChanged(ref _artistsCardsViewModel, value);
    }

    public CardsViewModel<YPlaylist> PlaylistsCardsViewModel
    {
        get => _playlistsCardsView;
        set => this.RaiseAndSetIfChanged(ref _playlistsCardsView, value);
    }
    
    public CardsViewModel<YAlbum> AlbumsCardsViewModel
    {
        get => _albumsCardsViewModel;
        set => this.RaiseAndSetIfChanged(ref _albumsCardsViewModel, value);
    }
    
    public ListViewModel<YTrack> PodcastEpisodesViewModel
    {
        get => _podcastEpisodesViewModel;
        set => this.RaiseAndSetIfChanged(ref _podcastEpisodesViewModel, value);
    }
    
    public ListViewModel<YTrack> TracksViewModel
    {
        get => _tracksViewModel;
        set => this.RaiseAndSetIfChanged(ref _tracksViewModel, value);
    }
#endregion

#region IRoutableViewModel

    public IScreen HostScreen { get; }

    // Unique identifier for the routable view model.
    public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

#endregion

#region Methods

    private void InitializeViewModels()
    {
        ArtistsCardsViewModel = new CardsViewModel<YArtist>(artist => new CardControlViewModel()
        {
            Header = artist.Name,
            SecondaryHeader = artist.Description,
        });
        PlaylistsCardsViewModel = new CardsViewModel<YPlaylist>(playlist => new CardControlViewModel()
        {
            Header = playlist.Title,
            SecondaryHeader = playlist.Description,
        });
        AlbumsCardsViewModel = new CardsViewModel<YAlbum>(album => new CardControlViewModel()
        {
            Header = album.Title,
            SecondaryHeader = album.ReleaseDate,
        });
        
        PodcastEpisodesViewModel = new ListViewModel<YTrack>();
        TracksViewModel = new ListViewModel<YTrack>();
    }

    private async Task SearchAsync()
    {
        await SearchArtistsAsync();
        await SearchPlaylistsAsync();
    }
    
    private async Task SearchArtistsAsync()
    {
        var api = new YandexMusicApi();
        var storage = AuthStorageService.GetInstance();

        var response = await api.Search.ArtistAsync(storage, SearchBarViewModel.SearchText);
        var artistsIds = response.Result.Artists.Results.ConvertAll(model => model.Id);

        var artists = new List<YArtist>();
        foreach (var id in artistsIds)
        {
            artists.Add((await api.Artist.GetAsync(storage, id)).Result.Artist);
        }
        
        ArtistsCardsViewModel.Source.Clear();
        ArtistsCardsViewModel.Source.AddRange(artists);
    }
    
    private async Task SearchPlaylistsAsync()
    {
        var api = new YandexMusicApi();
        var storage = AuthStorageService.GetInstance();

        var response = await api.Search.PlaylistAsync(storage, SearchBarViewModel.SearchText);
        var infos = response.Result.Playlists.Results.ConvertAll(model => new YPlaylist()
        {
            Kind = model.Kind,
            Owner = model.Owner,
        });

        var playlists = new List<YPlaylist>();
        foreach (var info in infos)
        {
            var playlistResponse = await api.Playlist.GetAsync(storage, info);
            playlists.Add(playlistResponse.Result);
        }
        
        PlaylistsCardsViewModel.Source.Clear();
        PlaylistsCardsViewModel.Source.AddRange(playlists);
    }

#endregion
}