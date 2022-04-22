using System.Reactive;
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
        SmartPlaylists = new SourceList<YPlaylist>();
        LoadSmartPlaylistsCommand = ReactiveCommand.CreateFromTask(LoadSmartPlaylistsAsync);
    }

#region Properties

    public SourceList<YPlaylist> SmartPlaylists { get; set; }

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
        var alice = await api.Playlist.OfTheDayAsync(storage);
        var podcasts = await api.Playlist.PremiereAsync(storage);
        var dejaVu = await api.Playlist.OfTheDayAsync(storage);
        var missed = await api.Playlist.PremiereAsync(storage);

        SmartPlaylists.Add(ofTheDay.Result);
        SmartPlaylists.Add(premiere.Result);
        SmartPlaylists.Add(alice.Result);
        SmartPlaylists.Add(podcasts.Result);
        SmartPlaylists.Add(dejaVu.Result);
        SmartPlaylists.Add(missed.Result);
    }

#endregion
}