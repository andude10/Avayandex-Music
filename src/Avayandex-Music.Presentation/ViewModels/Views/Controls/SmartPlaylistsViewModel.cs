using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avayandex_Music.Core.Services;
using DynamicData;
using ReactiveUI;
using Yandex.Music.Api;
using Yandex.Music.Api.Models.Playlist;

namespace Avayandex_Music.Presentation.ViewModels.Views.Controls;

public class SmartPlaylistsViewModel : ViewModelBase
{
    public SmartPlaylistsViewModel()
    {
        LoadSmartPlaylistsCommand = ReactiveCommand.CreateFromTask(LoadSmartPlaylistsAsync);

        _smartPlaylists.Connect()
            .Transform(playlist => new CardControlViewModel
            {
                Header = playlist.Title,
                SecondaryHeader = playlist.Description
            })
            .DisposeMany()
            .ObserveOn(RxApp.MainThreadScheduler)
            .Bind(out _playlistsCard)
            .Subscribe();
    }

#region Properties

    public ReadOnlyObservableCollection<CardControlViewModel> PlaylistsCard => _playlistsCard;

#endregion

#region Commands

    public ReactiveCommand<Unit, Unit> LoadSmartPlaylistsCommand { get; }

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

        if (ofTheDay != null) _smartPlaylists.Add(ofTheDay.Result);
        if (premiere != null) _smartPlaylists.Add(premiere.Result);
        if (alice != null) _smartPlaylists.Add(alice.Result);
        if (podcasts != null) _smartPlaylists.Add(podcasts.Result);
        if (dejaVu != null) _smartPlaylists.Add(dejaVu.Result);
        if (missed != null) _smartPlaylists.Add(missed.Result);
    }

#endregion

#region Fields

    private readonly SourceList<YPlaylist> _smartPlaylists = new();
    private readonly ReadOnlyObservableCollection<CardControlViewModel> _playlistsCard;

#endregion
}