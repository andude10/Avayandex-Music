using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avayandex_Music.Core.Services;
using DynamicData;
using ReactiveUI;
using Yandex.Music.Api;
using Yandex.Music.Api.Models.Playlist;

namespace Avayandex_Music.Presentation.ViewModels.Views.Controls;

public class SmartPlaylistsViewModel : ViewModelBase, IRoutableViewModel
{
    public SmartPlaylistsViewModel(IScreen screen)
    {
        HostScreen = screen;
        LoadSmartPlaylistsCommand = ReactiveCommand.CreateFromTask(LoadSmartPlaylistsAsync);
        NavigateToPlaylistCommand = ReactiveCommand.Create<string>(NavigateToPlaylist);

        _playlists.Connect()
            .Transform(playlist => new CardControlViewModel
            {
                Header = playlist.Title,
                SecondaryHeader = playlist.Description,
                Command = NavigateToPlaylistCommand,
                CommandParameter = playlist.Title
            })
            .DisposeMany()
            .ObserveOn(RxApp.MainThreadScheduler)
            .Bind(out _playlistsCollection)
            .Subscribe();
    }

#region Properties

    public ReadOnlyObservableCollection<CardControlViewModel> PlaylistsCollection => _playlistsCollection;

#endregion

#region Fields

    private readonly SourceList<YPlaylist> _playlists = new();
    private readonly ReadOnlyObservableCollection<CardControlViewModel> _playlistsCollection;

#endregion

#region Commands

    public ReactiveCommand<Unit, Unit> LoadSmartPlaylistsCommand { get; }
    public ReactiveCommand<string, Unit> NavigateToPlaylistCommand { get; }

#endregion

#region IRoutableViewModel

    public IScreen HostScreen { get; }

    // Unique identifier for the routable view model.
    public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

#endregion

#region Methods

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

        if (ofTheDay != null) _playlists.Add(ofTheDay.Result);
        if (premiere != null) _playlists.Add(premiere.Result);
        if (alice != null) _playlists.Add(alice.Result);
        if (podcasts != null) _playlists.Add(podcasts.Result);
        if (dejaVu != null) _playlists.Add(dejaVu.Result);
        if (missed != null) _playlists.Add(missed.Result);
    }

    private void NavigateToPlaylist(string playlistTitle)
    {
        var playlist = _playlists.Items.FirstOrDefault(p => p.Title == playlistTitle);

        if (playlist == null) return;

        var vm = new PlaylistViewModel(HostScreen);
        for (var i = 0; i < playlist.Tracks.Count(); i++) vm.TrackPlayer.Tracks.Add(playlist.Tracks[i].Track);
        HostScreen.Router.Navigate.Execute(vm);
    }

#endregion
}