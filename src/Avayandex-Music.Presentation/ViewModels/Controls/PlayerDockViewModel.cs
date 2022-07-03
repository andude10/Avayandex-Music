using System.Reactive.Linq;
using Avayandex_Music.Core.Playbacks;
using Avayandex_Music.Core.Players.Audio.Track;
using Splat;

namespace Avayandex_Music.Presentation.ViewModels.Controls;

public class PlayerDockViewModel : ViewModelBase
{
    private ITrackPlayer _trackPlayer;

    private PlayerDockViewModel(ITrackPlayer trackPlayer)
    {
        TrackPlayer = trackPlayer;

        var canPlayerCommandsExecute = this.WhenAnyObservable(vm => vm.TrackPlayer.PlayAsyncCommand.CanExecute,
                vm => vm.TrackPlayer.PauseCommand.CanExecute, (b1, b2) => b1 | b2)
            .ObserveOn(RxApp.MainThreadScheduler)
            .Select(x => x);
        PlayOrPauseCommand = ReactiveCommand.CreateFromTask(PlayOrPause, canPlayerCommandsExecute);
    }

#region Commands

    public ReactiveCommand<Unit, Unit> PlayOrPauseCommand { get; }

#endregion

#region Properties

    public ITrackPlayer TrackPlayer
    {
        get => _trackPlayer;
        private set => this.RaiseAndSetIfChanged(ref _trackPlayer, value);
    }

    public static PlayerDockViewModel Instance { get; private set; }
        = new(Locator.Current.GetService<ITrackPlayer>()
              ?? throw new InvalidOperationException());

#endregion

#region Methods

    public static void Create(ITrackPlayer trackPlayer)
    {
        Instance = new PlayerDockViewModel(trackPlayer);
    }

    public static void SetPlayer(ITrackPlayer trackPlayer)
    {
        if (Instance.TrackPlayer == trackPlayer) return;

        Instance.TrackPlayer.Dispose();

        Instance.TrackPlayer = trackPlayer;
    }

    private async Task PlayOrPause()
    {
        if (TrackPlayer.State != PlaybackState.Playing)
            await TrackPlayer.PlayAsyncCommand.Execute();
        else
            TrackPlayer.PauseCommand.Execute().Subscribe();
    }

#endregion
}