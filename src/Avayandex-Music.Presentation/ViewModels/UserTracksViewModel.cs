using System.Collections.ObjectModel;
using System.Reactive.Linq;
using Avayandex_Music.Core.Players.Audio.Track;
using Avayandex_Music.Core.Services;
using Avayandex_Music.Presentation.ViewModels.Controls;
using DynamicData;
using Splat;
using Yandex.Music.Api;
using Yandex.Music.Api.Models.Track;

namespace Avayandex_Music.Presentation.ViewModels;

public class UserTracksViewModel : ViewModelBase, IRoutableViewModel
{
#region Fields

    private readonly ReadOnlyObservableCollection<YTrack> _tracksCollection;

#endregion

    public UserTracksViewModel(IScreen screen)
    {
        HostScreen = screen;
        TrackPlayer = Locator.Current.GetService<ITrackPlayer>()
                      ?? throw new InvalidOperationException();
        TrackPlayer.Tracks.Connect()
            .ObserveOn(RxApp.MainThreadScheduler)
            .Bind(out _tracksCollection)
            .Subscribe();

        StartPlayCommand = ReactiveCommand.Create(StartPlay);
        LoadTracksCommand = ReactiveCommand.CreateFromTask(LoadTracks);
    }

#region Commands

    public ReactiveCommand<Unit, Unit> StartPlayCommand { get; }
    public ReactiveCommand<Unit, Unit> LoadTracksCommand { get; }

#endregion

#region Methods

    private void StartPlay()
    {
        PlayerDockViewModel.SetPlayer(TrackPlayer);
        TrackPlayer.SelectedTrack = TrackPlayer.Tracks.Items.First();
    }

    private async Task LoadTracks()
    {
        var api = new YandexMusicApi();
        var storage = AuthStorageService.GetInstance();

        var getTracksResponse = await api.Library.GetLikedTracksAsync(storage);
        var tracksIds = getTracksResponse.Result.Library.Tracks.ConvertAll(model => model.Id).ToList();

        var getRequestsTasks = tracksIds.Select(id => api.Track.GetAsync(storage, id)).ToList();

        while (getRequestsTasks.Any())
        {
            var finishedTask = await Task.WhenAny(getRequestsTasks);
            getRequestsTasks.Remove(finishedTask);

            var response = finishedTask.Result;

            if (response != null) TrackPlayer.Tracks.AddRange(response.Result);
        }
    }

#endregion

#region Properties

    public ReadOnlyObservableCollection<YTrack> TracksCollection => _tracksCollection;
    public ITrackPlayer TrackPlayer { get; }

#endregion

#region IRoutableViewModel

    public IScreen HostScreen { get; }

    // Unique identifier for the routable view model.
    public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

#endregion
}