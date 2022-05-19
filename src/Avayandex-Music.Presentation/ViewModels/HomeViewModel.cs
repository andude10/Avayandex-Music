using Avayandex_Music.Core.Services;
using Avayandex_Music.Presentation.ViewModels.Controls;
using DynamicData;
using Yandex.Music.Api;
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
        await LoadSmartPlaylistsAsync();
        await LoadPodcastsAsync();
    }

    private async Task LoadSmartPlaylistsAsync()
    {
        var api = new YandexMusicApi();
        var storage = AuthStorageService.GetInstance();

        var ofTheDay = await api.Playlist.OfTheDayAsync(storage);
        var premiere = await api.Playlist.PremiereAsync(storage);
        var alice = await api.Playlist.AliceAsync(storage);
        var podcasts = await api.Playlist.PodcastsAsync(storage);
        var dejaVu = await api.Playlist.DejaVuAsync(storage);
        var missed = await api.Playlist.MissedAsync(storage);

        if (ofTheDay != null) SmartPlaylistsViewModel.Source.Add(ofTheDay.Result);
        if (premiere != null) SmartPlaylistsViewModel.Source.Add(premiere.Result);
        if (alice != null) SmartPlaylistsViewModel.Source.Add(alice.Result);
        if (podcasts != null) SmartPlaylistsViewModel.Source.Add(podcasts.Result);
        if (dejaVu != null) SmartPlaylistsViewModel.Source.Add(dejaVu.Result);
        if (missed != null) SmartPlaylistsViewModel.Source.Add(missed.Result);
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