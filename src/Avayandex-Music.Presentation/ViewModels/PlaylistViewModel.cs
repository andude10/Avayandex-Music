using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using Avayandex_Music.Core.Players.Audio.Track;
using Avayandex_Music.Presentation.ViewModels.Controls;
using DynamicData;
using ReactiveUI;
using Splat;
using Yandex.Music.Api.Models.Track;

namespace Avayandex_Music.Presentation.ViewModels;

public class PlaylistViewModel : ViewModelBase, IRoutableViewModel
{
#region Fields

    private readonly ReadOnlyObservableCollection<YTrack> _tracksCollection;

#endregion

    public PlaylistViewModel(IScreen screen)
    {
        HostScreen = screen;
        TrackPlayer = Locator.Current.GetService<ITrackPlayer>()
                      ?? throw new InvalidOperationException();
        TrackPlayer.Tracks.Connect()
            .ObserveOn(RxApp.MainThreadScheduler)
            .Bind(out _tracksCollection)
            .Subscribe();

        StartPlayCommand = ReactiveCommand.Create(StartPlay);
    }

#region Commands

    public ReactiveCommand<Unit, Unit> StartPlayCommand { get; }

#endregion

#region Methods

    private void StartPlay()
    {
        PlayerDockViewModel.SetPlayer(TrackPlayer);
        PlayerDockViewModel.Instance.TrackPlayer.SelectCommand.Execute(0).Subscribe();
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