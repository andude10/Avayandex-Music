using Avayandex_Music.Core.Services;
using Avayandex_Music.Presentation.ViewModels.Controls;
using DynamicData;
using Yandex.Music.Api;
using Yandex.Music.Api.Models.Album;
using Yandex.Music.Api.Models.Artist;
using Yandex.Music.Api.Models.Common;
using Yandex.Music.Api.Models.Common.Cover;
using Yandex.Music.Api.Models.Playlist;
using Yandex.Music.Api.Models.Search;
using Yandex.Music.Api.Models.Search.Album;
using Yandex.Music.Api.Models.Search.Artist;
using Yandex.Music.Api.Models.Search.Playlist;
using Yandex.Music.Api.Models.Search.PodcastEpisode;
using Yandex.Music.Api.Models.Search.Track;
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
    private ListViewModel<YTrack> _podcastsCardsViewModel;
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

    public ListViewModel<YTrack> PodcastCardsViewModel
    {
        get => _podcastsCardsViewModel;
        set => this.RaiseAndSetIfChanged(ref _podcastsCardsViewModel, value);
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

        PodcastCardsViewModel = new ListViewModel<YTrack>();
        TracksViewModel = new ListViewModel<YTrack>();
    }

    private async Task SearchAsync()
    {
        var api = new YandexMusicApi();
        var storage = AuthStorageService.GetInstance();

        var searchResponse = await api.Search.SearchAsync(storage, SearchBarViewModel.SearchText, YSearchType.All);

        if (searchResponse == null) return;

        var downloadTasks = new List<Task>();

        if (searchResponse.Result.Podcast_Episodes != null)
            downloadTasks.Add(DownloadPodcastEpisodeAsync(searchResponse.Result.Podcast_Episodes));
        if (searchResponse.Result.Artists != null)
            downloadTasks.Add(DownloadArtistsAsync(searchResponse.Result.Artists));
        if (searchResponse.Result.Albums != null)
            downloadTasks.Add(DownloadAlbumsAsync(searchResponse.Result.Albums));
        if (searchResponse.Result.Playlists != null)
            downloadTasks.Add(DownloadPlaylistsAsync(searchResponse.Result.Playlists));
        if (searchResponse.Result.Tracks != null)
            downloadTasks.Add(DownloadTracksAsync(searchResponse.Result.Tracks));

        while (downloadTasks.Any())
        {
            var finishedTask = await Task.WhenAny(downloadTasks);
            downloadTasks.Remove(finishedTask);
        }
    }

    private async Task DownloadPodcastEpisodeAsync(YSearchResult<YSearchPodcastEpisodeModel> searchResult)
    {
        var api = new YandexMusicApi();
        var storage = AuthStorageService.GetInstance();

        var podcastsIds = searchResult.Results.ConvertAll(model => model.Id);

        var getRequestsTasks = podcastsIds.Select(id => api.Track.GetAsync(storage, id)).ToList();

        while (getRequestsTasks.Any())
        {
            var finishedTask = await Task.WhenAny(getRequestsTasks);
            getRequestsTasks.Remove(finishedTask);

            var response = await finishedTask;

            if (response != null) TracksViewModel.Source.AddRange(response.Result);
        }
    }

    private async Task DownloadArtistsAsync(YSearchResult<YSearchArtistModel> searchResult)
    {
        var api = new YandexMusicApi();
        var storage = AuthStorageService.GetInstance();

        var artistsIds = searchResult.Results.ConvertAll(model => model.Id);

        var getRequestsTasks = artistsIds.Select(id => api.Artist.GetAsync(storage, id)).ToList();

        while (getRequestsTasks.Any())
        {
            var finishedTask = await Task.WhenAny(getRequestsTasks);
            getRequestsTasks.Remove(finishedTask);

            var response = await finishedTask;

            if (response != null) ArtistsCardsViewModel.Source.Add(response.Result.Artist);
        }
    }

    private async Task DownloadPlaylistsAsync(YSearchResult<YSearchPlaylistModel> searchResult)
    {
        var api = new YandexMusicApi();
        var storage = AuthStorageService.GetInstance();

        var infos = searchResult.Results.ConvertAll(model => new YPlaylist
        {
            Kind = model.Kind,
            Owner = model.Owner
        });

        var getRequestsTasks = infos.Select(info => api.Playlist.GetAsync(storage, info)).ToList();

        while (getRequestsTasks.Any())
        {
            var finishedTask = await Task.WhenAny(getRequestsTasks);
            getRequestsTasks.Remove(finishedTask);

            var response = await finishedTask;

            if (response != null) PlaylistsCardsViewModel.Source.Add(response.Result);
        }
    }

    private async Task DownloadAlbumsAsync(YSearchResult<YSearchAlbumModel> searchResult)
    {
        var api = new YandexMusicApi();
        var storage = AuthStorageService.GetInstance();

        var albumsIds = searchResult.Results.ConvertAll(model => model.Id);

        var getRequestsTasks = albumsIds.Select(id => api.Album.GetAsync(storage, id)).ToList();

        while (getRequestsTasks.Any())
        {
            var finishedTask = await Task.WhenAny(getRequestsTasks);
            getRequestsTasks.Remove(finishedTask);

            var response = await finishedTask;

            if (response != null) AlbumsCardsViewModel.Source.Add(response.Result);
        }
    }

    private async Task DownloadTracksAsync(YSearchResult<YSearchTrackModel> searchResult)
    {
        var api = new YandexMusicApi();
        var storage = AuthStorageService.GetInstance();

        var tracksIds = searchResult.Results.ConvertAll(model => model.Id);

        var getRequestsTasks = tracksIds.Select(id => api.Track.GetAsync(storage, id)).ToList();

        while (getRequestsTasks.Any())
        {
            var finishedTask = await Task.WhenAny(getRequestsTasks);
            getRequestsTasks.Remove(finishedTask);

            var response = await finishedTask;

            if (response != null) TracksViewModel.Source.AddRange(response.Result);
        }
    }

#endregion
}