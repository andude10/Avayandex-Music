using Avayandex_Music.Core.Services;
using Avayandex_Music.Presentation.ViewModels.Controls;
using DynamicData;
using Yandex.Music.Api;
using Yandex.Music.Api.Models.Common;
using Yandex.Music.Api.Models.Playlist;
using Yandex.Music.Api.Models.Track;

namespace Avayandex_Music.Presentation.ViewModels;

public class HomeViewModel : ViewModelBase, IRoutableViewModel
{
    public HomeViewModel(IScreen screen)
    {
        HostScreen = screen;
        LoadDataCommand = ReactiveCommand.CreateFromTask(LoadDataAsync);

        var playlistCardCommand = ReactiveCommand.Create<string>(playlistTitle =>
        {
            var playlist = SmartPlaylistsViewModel?.Source.Items.FirstOrDefault(p => p.Title == playlistTitle);

            if (playlist == null) return;

            var vm = new PlaylistViewModel(HostScreen);
            for (var i = 0; i < playlist.Tracks.Count(); i++) vm.TrackPlayer.Tracks.Add(playlist.Tracks[i].Track);
            HostScreen.Router.Navigate.Execute(vm);
        });

        SmartPlaylistsViewModel = new CardsViewModel<YPlaylist>(playlist => new CardControlViewModel
        {
            Header = playlist.Title,
            SecondaryHeader = playlist.Description,
            Command = playlistCardCommand,
            CommandParameter = playlist.Title
        });

        RecommendedEpisodesViewModel = new ListViewModel<YTrack>();
        SearchBarViewModel = new SearchBarViewModel(screen);
    }

#region Commands

    /// <summary>
    ///     Command to start a background download of data
    /// </summary>
    public ReactiveCommand<Unit, Unit> LoadDataCommand { get; }

#endregion

#region Properties

    public SearchBarViewModel SearchBarViewModel { get; set; }
    public CardsViewModel<YPlaylist> SmartPlaylistsViewModel { get; set; }
    public ListViewModel<YTrack> RecommendedEpisodesViewModel { get; set; }

#endregion

#region IRoutableViewModel

    public IScreen HostScreen { get; }

    // Unique identifier for the routable view model.
    public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

#endregion

#region Methods

    /// <summary>
    ///     Starts a background download of data
    /// </summary>
    private async Task LoadDataAsync()
    {
        var loadSmartPlaylists = LoadSmartPlaylistsAsync();
        var loadPodcasts = LoadPodcastsAsync();

        await Task.WhenAll(loadSmartPlaylists, loadPodcasts);
    }

    private async Task LoadSmartPlaylistsAsync()
    {
        var api = new YandexMusicApi();
        var storage = AuthStorageService.GetInstance();

        var requestsTasks = new List<Task<YResponse<YPlaylist>?>>
        {
            api.Playlist.OfTheDayAsync(storage),
            api.Playlist.PremiereAsync(storage),
            api.Playlist.AliceAsync(storage),
            api.Playlist.PodcastsAsync(storage),
            api.Playlist.DejaVuAsync(storage),
            api.Playlist.MissedAsync(storage),
        };

        while (requestsTasks.Any())
        {
            var task = await Task.WhenAny(requestsTasks);
            requestsTasks.Remove(task);
            
            var response = await task;

            if (response != null)
            {
                SmartPlaylistsViewModel.Source.Add(response.Result);
            }
        }
    }

    private async Task LoadPodcastsAsync()
    {
        var api = new YandexMusicApi();
        var storage = AuthStorageService.GetInstance();

        var response = await api.Playlist.PodcastsAsync(storage);

        if (response != null)
            foreach (var episode in response.Result.Tracks)
                RecommendedEpisodesViewModel.Source.Add(episode.Track);
    }

#endregion
}