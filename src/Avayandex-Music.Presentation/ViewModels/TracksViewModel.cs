using System.Collections.ObjectModel;
using System.Reactive.Linq;
using Avayandex_Music.Core.Playbacks;
using Avayandex_Music.Core.Players.Audio.Track;
using Avayandex_Music.Presentation.ViewModels.Controls;
using DynamicData;
using Splat;
using Yandex.Music.Api.Models.Track;

namespace Avayandex_Music.Presentation.ViewModels;

public class TracksViewModel : ViewModelBase, IRoutableViewModel
{
#region Fields

    private readonly ReadOnlyObservableCollection<YTrack> _tracksCollection;

#endregion

    public TracksViewModel(IScreen hostScreen)
    {
        HostScreen = hostScreen;
        TrackPlayer = Locator.Current.GetService<ITrackPlayer>()
                      ?? throw new InvalidOperationException();

        TrackPlayer.Tracks.Connect()
            .ObserveOn(RxApp.MainThreadScheduler)
            .Bind(out _tracksCollection)
            .Subscribe();

        PlayOrPauseCommand = ReactiveCommand.CreateFromTask<YTrack>(PlayOrPause);
    }

#region Commands

    public ReactiveCommand<YTrack, Unit> PlayOrPauseCommand { get; }

#endregion

#region Methods

    private async Task PlayOrPause(YTrack track)
    {
        if (PlayerDockViewModel.Instance.TrackPlayer != TrackPlayer) PlayerDockViewModel.SetPlayer(TrackPlayer);

        var trackPosition = TrackPlayer.Tracks.Items.IndexOf(track);
        TrackPlayer.SelectCommand.Execute(trackPosition).Subscribe();

        if (TrackPlayer.State != PlaybackState.Playing)
            await TrackPlayer.PlayAsyncCommand.Execute();
        else
            TrackPlayer.PauseCommand.Execute().Subscribe();
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