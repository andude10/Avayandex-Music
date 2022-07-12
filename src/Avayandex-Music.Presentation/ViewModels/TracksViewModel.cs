using System.Collections.ObjectModel;
using System.Reactive.Linq;
using Avalonia.Controls;
using Avalonia.Controls.Selection;
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
        _selection = new SelectionModel<YTrack>();

        TrackPlayer.Tracks.Connect()
            .ObserveOn(RxApp.MainThreadScheduler)
            .Bind(out _tracksCollection)
            .Subscribe();

        PlayOrPauseCommand = ReactiveCommand.CreateFromTask<YTrack>(PlayOrPause);

        this.WhenAnyValue(vm => vm.SelectedItem)
            .WhereNotNull()
            .InvokeCommand(PlayOrPauseCommand);
    }

#region Commands

    public ReactiveCommand<YTrack, Unit> PlayOrPauseCommand { get; }

#endregion

#region Methods

    private async Task PlayOrPause(YTrack track)
    {
        if (PlayerDockViewModel.Instance.TrackPlayer != TrackPlayer) PlayerDockViewModel.SetPlayer(TrackPlayer);

        var trackIndex = TrackPlayer.Tracks.Items.IndexOf(track);

        if (!track.Equals(SelectedItem)) Selection.Select(trackIndex);

        TrackPlayer.SelectCommand.Execute(trackIndex).Subscribe();

        if (TrackPlayer.State != PlaybackState.Playing)
            await TrackPlayer.PlayAsyncCommand.Execute();
        else
            TrackPlayer.PauseCommand.Execute().Subscribe();
    }

#endregion

#region Fields

    private readonly ReadOnlyObservableCollection<YTrack> _tracksCollection;
    private YTrack? _selectedItem;
    private ISelectionModel _selection;
    private SelectionMode _selectionMode;

#endregion

#region Properties

    public ReadOnlyObservableCollection<YTrack> TracksCollection => _tracksCollection;

    public ITrackPlayer TrackPlayer { get; }

    public ISelectionModel Selection
    {
        get => _selection;
        set => this.RaiseAndSetIfChanged(ref _selection, value);
    }

    public SelectionMode SelectionMode
    {
        get => _selectionMode;
        set => this.RaiseAndSetIfChanged(ref _selectionMode, value);
    }

    public YTrack? SelectedItem
    {
        get => _selectedItem;
        set => this.RaiseAndSetIfChanged(ref _selectedItem, value);
    }

#endregion

#region IRoutableViewModel

    public IScreen HostScreen { get; }

    // Unique identifier for the routable view model.
    public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

#endregion
}