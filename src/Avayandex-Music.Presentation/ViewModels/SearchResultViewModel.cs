using Avayandex_Music.Core.Services;
using Avayandex_Music.Presentation.ViewModels.Controls;
using DynamicData;
using Yandex.Music.Api;
using Yandex.Music.Api.Models.Album;
using Yandex.Music.Api.Models.Artist;
using Yandex.Music.Api.Models.Common.Cover;
using Yandex.Music.Api.Models.Playlist;
using Yandex.Music.Api.Models.Track;

namespace Avayandex_Music.Presentation.ViewModels;

public class SearchResultViewModel : ViewModelBase, IRoutableViewModel
{
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
    ///     Searching for data on the entered text
    /// </summary>
    public ReactiveCommand<Unit, Unit> SearchCommand { get; }

#endregion

#region Fields

    private SearchBarViewModel _searchBarViewModel;
    private CardsViewModel<YArtist> _artistsCardsViewModel;
    private CardsViewModel<YPlaylist> _playlistsCardsView;
    private CardsViewModel<YAlbum> _albumsCardsViewModel;
    private ListViewModel<YTrack> _tracksViewModel;

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
        ArtistsCardsViewModel = new CardsViewModel<YArtist>(artist => new CardControlViewModel
        {
            Header = artist.Name,
            SecondaryHeader = string.Join(", ", artist.Genres),
            Cover = artist.Cover
        });
        PlaylistsCardsViewModel = new CardsViewModel<YPlaylist>(playlist => new CardControlViewModel
        {
            Header = playlist.Title,
            Cover = playlist.Cover
        });
        AlbumsCardsViewModel = new CardsViewModel<YAlbum>(album => new CardControlViewModel
        {
            Header = album.Title,
            SecondaryHeader = string.Join(", ", album.Artists.Select(artist => artist.Name)),
            Cover = new YCoverImage {Uri = album.CoverUri}
        });

        TracksViewModel = new ListViewModel<YTrack>();
    }

    private async Task SearchAsync()
    {
        var searchTasks = new List<Task>
        {
            SearchArtistsAsync(),
            SearchPlaylistsAsync(),
            SearchAlbumsAsync(),
            SearchTracksAsync()
        };

        while (searchTasks.Any())
        {
            var finishedTask = await Task.WhenAny(searchTasks);
            searchTasks.Remove(finishedTask);
        }
    }

    private async Task SearchArtistsAsync()
    {
        var api = new YandexMusicApi();
        var storage = AuthStorageService.GetInstance();

        var searchResponse = await api.Search.ArtistAsync(storage, SearchBarViewModel.SearchText);
        var artistsIds = searchResponse.Result.Artists.Results.ConvertAll(model => model.Id);

        var getRequestsTasks = artistsIds.Select(id => api.Artist.GetAsync(storage, id)).ToList();

        while (getRequestsTasks.Any())
        {
            var finishedTask = await Task.WhenAny(getRequestsTasks);
            getRequestsTasks.Remove(finishedTask);

            var response = finishedTask.Result;

            if (response != null) ArtistsCardsViewModel.Source.Add(response.Result.Artist);
        }
    }

    private async Task SearchPlaylistsAsync()
    {
        var api = new YandexMusicApi();
        var storage = AuthStorageService.GetInstance();

        var searchResponse = await api.Search.PlaylistAsync(storage, SearchBarViewModel.SearchText);
        var infos = searchResponse.Result.Playlists.Results.ConvertAll(model => new YPlaylist
        {
            Kind = model.Kind,
            Owner = model.Owner
        });

        var getRequestsTasks = infos.Select(info => api.Playlist.GetAsync(storage, info)).ToList();

        while (getRequestsTasks.Any())
        {
            var finishedTask = await Task.WhenAny(getRequestsTasks);
            getRequestsTasks.Remove(finishedTask);

            var response = finishedTask.Result;

            if (response != null) PlaylistsCardsViewModel.Source.Add(response.Result);
        }
    }

    private async Task SearchAlbumsAsync()
    {
        var api = new YandexMusicApi();
        var storage = AuthStorageService.GetInstance();

        var searchResponse = await api.Search.AlbumsAsync(storage, SearchBarViewModel.SearchText);
        var albumsIds = searchResponse.Result.Albums.Results.ConvertAll(model => model.Id);

        var getRequestsTasks = albumsIds.Select(id => api.Album.GetAsync(storage, id)).ToList();

        while (getRequestsTasks.Any())
        {
            var finishedTask = await Task.WhenAny(getRequestsTasks);
            getRequestsTasks.Remove(finishedTask);

            var response = finishedTask.Result;

            if (response != null) AlbumsCardsViewModel.Source.Add(response.Result);
        }
    }

    private async Task SearchTracksAsync()
    {
        var api = new YandexMusicApi();
        var storage = AuthStorageService.GetInstance();

        var searchResponse = await api.Search.TrackAsync(storage, SearchBarViewModel.SearchText);
        var tracksIds = searchResponse.Result.Tracks.Results.ConvertAll(model => model.Id);

        var getRequestsTasks = tracksIds.Select(id => api.Track.GetAsync(storage, id)).ToList();

        while (getRequestsTasks.Any())
        {
            var finishedTask = await Task.WhenAny(getRequestsTasks);
            getRequestsTasks.Remove(finishedTask);

            var response = finishedTask.Result;

            if (response != null) TracksViewModel.Source.AddRange(response.Result);
        }
    }

#endregion
}