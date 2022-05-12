using System;
using Avayandex_Music.Core.Players.Audio.Track;
using ReactiveUI;
using Splat;

namespace Avayandex_Music.Presentation.ViewModels.Controls;

public class PlayerDockViewModel : ViewModelBase
{
    private ITrackPlayer _trackPlayer;

    private PlayerDockViewModel(ITrackPlayer trackPlayer)
    {
        TrackPlayer = trackPlayer;
    }

#region Properties

    public ITrackPlayer TrackPlayer
    {
        get => _trackPlayer;
        private set => this.RaiseAndSetIfChanged(ref _trackPlayer, value);
    }

    public static PlayerDockViewModel Instance { get; private set; }
        = new PlayerDockViewModel(Locator.Current.GetService<ITrackPlayer>()
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

#endregion
}