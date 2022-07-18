using System.Collections.ObjectModel;
using System.Reactive.Linq;
using Avalonia.Controls;
using Avayandex_Music.Core.Playbacks;
using Avayandex_Music.Core.Players.Audio.Track;
using Avayandex_Music.Presentation.ViewModels.Controls;
using DynamicData;
using Splat;
using Yandex.Music.Api.Models.Track;

namespace Avayandex_Music.Presentation.ViewModels;

public class TracksViewModel : ViewModelBase, IRoutableViewModel
{
    public TracksViewModel(IScreen hostScreen)
    {
        HostScreen = hostScreen;
        TrackPlayer = Locator.Current.GetService<ITrackPlayer>()
                      ?? throw new InvalidOperationException();
        _selectionMode = SelectionMode.Single;

        TrackPlayer.Tracks.Connect()
            .ObserveOn(RxApp.MainThreadScheduler)
            .Bind(out _tracksCollection)
            .Subscribe();

        PlayOrPausePlayerCommand = ReactiveCommand.CreateFromTask<YTrack?>(PlayOrPausePlayer);
        SelectTrackCommand = ReactiveCommand.CreateFromTask<YTrack>(SelectTrack);

        this.WhenAnyValue(vm => vm.TrackPlayer.SelectedTrack)
            .InvokeCommand(PlayOrPausePlayerCommand);
    }

#region Commands

    public ReactiveCommand<YTrack?, Unit> PlayOrPausePlayerCommand { get; }

    public ReactiveCommand<YTrack, Unit> SelectTrackCommand { get; }

#endregion

#region Methods

    private async Task PlayOrPausePlayer(YTrack? track)
    {
        if ((track == null) & (TrackPlayer.SelectedTrack == null)) return;
        if (PlayerDockViewModel.Instance.TrackPlayer != TrackPlayer) PlayerDockViewModel.SetPlayer(TrackPlayer);

        if (track != null && !track.Equals(TrackPlayer.SelectedTrack)) TrackPlayer.SelectedTrack = track;

        if (TrackPlayer.State != PlaybackState.Playing)
            await TrackPlayer.PlayAsyncCommand.Execute();
        else
            TrackPlayer.PauseCommand.Execute().Subscribe();
    }

    private async Task SelectTrack(YTrack track)
    {
        if (track.Equals(TrackPlayer.SelectedTrack))
            await PlayOrPausePlayerCommand.Execute();
        else
            TrackPlayer.SelectedTrack = track;
    }

#endregion

#region Fields

    private readonly ReadOnlyObservableCollection<YTrack> _tracksCollection;
    private SelectionMode _selectionMode;

#endregion

#region Properties

    public ReadOnlyObservableCollection<YTrack> TracksCollection => _tracksCollection;

    public ITrackPlayer TrackPlayer { get; }

    public SelectionMode SelectionMode
    {
        get => _selectionMode;
        set => this.RaiseAndSetIfChanged(ref _selectionMode, value);
    }

#endregion

#region IRoutableViewModel

    public IScreen HostScreen { get; }

    // Unique identifier for the routable view model.
    public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

#endregion
}