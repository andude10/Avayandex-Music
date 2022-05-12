using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avayandex_Music.Core.Players.Audio.Track;
using Avayandex_Music.Core.Services;
using Avayandex_Music.Presentation.ViewModels.Controls;
using DynamicData;
using ReactiveUI;
using Splat;
using Yandex.Music.Api;

namespace Avayandex_Music.Presentation.ViewModels;

public class HomeViewModel : ViewModelBase, IRoutableViewModel
{
#region Fields

    private readonly ITrackPlayer _trackPlayer;

#endregion

    public HomeViewModel(IScreen screen)
    {
        HostScreen = screen;

        FindTrackCommand = ReactiveCommand.CreateFromTask(FindTrack);
        PlayCommand = ReactiveCommand.CreateFromTask(PlayTrackAsync);
        StopCommand = ReactiveCommand.Create(StopTrack);
        LoadDataCommand = ReactiveCommand.CreateFromTask(LoadDataAsync);

        _trackPlayer = Locator.Current.GetService<ITrackPlayer>()
                       ?? throw new InvalidOperationException();
        SmartPlaylistsViewModel = new SmartPlaylistsViewModel(HostScreen);
    }

#region Properties

    public SmartPlaylistsViewModel SmartPlaylistsViewModel { get; set; }

#endregion

#region IRoutableViewModel

    public IScreen HostScreen { get; }

    // Unique identifier for the routable view model.
    public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

#endregion

#region Commands

    public ReactiveCommand<Unit, Unit> FindTrackCommand { get; }
    public ReactiveCommand<Unit, Unit> PlayCommand { get; }
    public ReactiveCommand<Unit, Unit> StopCommand { get; }

    /// <summary>
    ///     Command to start a background download of data
    /// </summary>
    public ReactiveCommand<Unit, Unit> LoadDataCommand { get; }

#endregion

#region Methods

    // test method
    public async Task FindTrack()
    {
        var authStorage = AuthStorageService.GetInstance();
        var api = new YandexMusicApi();
        var getTrackId = (await api.Library.GetLikedTracksAsync(authStorage)).Result.Library.Tracks.First().Id;
        var track = (await api.Track.GetAsync(authStorage, getTrackId)).Result.First();
        _trackPlayer.Tracks.Add(track);
    }

    // test method
    private async Task PlayTrackAsync()
    {
        _trackPlayer.SelectCommand.Execute(0).Subscribe();
        await _trackPlayer.PlayAsyncCommand.Execute();
    }

    /// <summary>
    ///     Starts a background download of data
    /// </summary>
    private async Task LoadDataAsync()
    {
        await SmartPlaylistsViewModel.LoadSmartPlaylistsCommand.Execute();
    }

    // test method
    private void StopTrack()
    {
        _trackPlayer.PauseCommand.Execute().Subscribe();
    }

#endregion
}