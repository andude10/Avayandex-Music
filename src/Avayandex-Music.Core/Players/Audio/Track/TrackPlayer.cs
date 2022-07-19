using System.Reactive.Linq;
using Avayandex_Music.Core.Playbacks;
using Avayandex_Music.Core.Playbacks.Audio;
using Avayandex_Music.Core.Storages;
using DynamicData;

namespace Avayandex_Music.Core.Players.Audio.Track;

public class TrackPlayer : ReactiveObject, ITrackPlayer
{
    public TrackPlayer(Storage storage, PlaybackAudio playbackAudio)
    {
        _storage = storage;
        _playbackAudio = playbackAudio;

        this.WhenAnyValue(player => player.PlaybackAudio)
            .Subscribe(playback =>
            {
                StateChange = this.WhenAnyValue(player => player.PlaybackAudio.State, player => player.ActualTrack);
            });

        var isSelectedTrackNotNull = this.WhenAnyValue(player => player.SelectedTrack)
            .ObserveOn(RxApp.MainThreadScheduler)
            .Select(x => x != null);

        PlayAsyncCommand = ReactiveCommand.CreateFromTask(LoadAndPlayAsync, isSelectedTrackNotNull);
        PauseCommand = ReactiveCommand.Create(Pause, isSelectedTrackNotNull);
        StopCommand = ReactiveCommand.Create(Stop, isSelectedTrackNotNull);

        SelectNextCommand = ReactiveCommand.Create(SelectNext,
            Tracks.Connect()
                .ToCollection()
                .Select(items => items.Any() && !items.Last().Equals(SelectedTrack)));
        SelectPreviousCommand = ReactiveCommand.Create(SelectPrevious,
            Tracks.Connect()
                .ToCollection()
                .Select(items => items.Any() && !items.Last().Equals(SelectedTrack)));
    }

#region Fields

    private readonly Storage _storage;
    private PlaybackAudio _playbackAudio;
    private YTrack? _selectedTrack;
    private YTrack _actualTrack = new();

#endregion

#region Properties

    public PlaybackState State => _playbackAudio.State;

    public IObservable<(PlaybackState, YTrack)> StateChange { get; protected set; }

    /// <summary>
    ///     All tracks in the player
    /// </summary>
    public SourceList<YTrack> Tracks { get; } = new();

    /// <summary>
    ///     The track selected in the player. (It plays, stops,
    ///     etc.) To select a track, use the Select* methods
    /// </summary>
    public YTrack? SelectedTrack
    {
        get => _selectedTrack;
        set
        {
            if (value != null && !Tracks.Items.Contains(value)) Tracks.Add(value);
            this.RaiseAndSetIfChanged(ref _selectedTrack, value);
        }
    }

    private YTrack ActualTrack
    {
        get => _actualTrack;
        set => this.RaiseAndSetIfChanged(ref _actualTrack, value);
    }

    private PlaybackAudio PlaybackAudio
    {
        get => _playbackAudio;
        set => this.RaiseAndSetIfChanged(ref _playbackAudio, value);
    }

#endregion

#region Commands

    /// <summary>
    ///     Command to load the selected track
    ///     (if it hasn't been loaded before) and plays it.
    /// </summary>
    public ReactiveCommand<Unit, Unit> PlayAsyncCommand { get; }

    /// <summary>
    ///     Command to Pause the selected track
    ///     CanExecute is false when the track is already stopped.
    /// </summary>
    public ReactiveCommand<Unit, Unit> PauseCommand { get; }

    /// <summary>
    ///     Command to stop the selected track
    ///     CanExecute is false when the track is already stopped.
    /// </summary>
    public ReactiveCommand<Unit, Unit> StopCommand { get; }

    /// <summary>
    ///     Command to play the next or first track in the list.
    ///     CanExecute equals false, when the last track in the list is playing
    /// </summary>
    public ReactiveCommand<Unit, Unit> SelectNextCommand { get; }

    /// <summary>
    ///     Command to play the previous track in the list.
    ///     CanExecute is false when the last track in the list is playing
    /// </summary>
    public ReactiveCommand<Unit, Unit> SelectPreviousCommand { get; }

#endregion

#region Methods

    private async Task LoadAndPlayAsync()
    {
        if (SelectedTrack == null)
            throw new InvalidOperationException("SelectedTrack is null");

        if (ActualTrack.Equals(SelectedTrack))
        {
            _playbackAudio.Play();
        }
        else
        {
            ActualTrack = SelectedTrack;

            var filePath = await _storage.LoadTrackAsync(SelectedTrack);

            PlaybackAudio.Dispose();
            PlaybackAudio = _playbackAudio.CreatePlayback(filePath);
            PlaybackAudio.Play();
        }
    }

    private void Pause()
    {
        if (SelectedTrack == null) throw new InvalidOperationException();
        _playbackAudio.Pause();
    }

    private void Stop()
    {
        if (SelectedTrack == null) throw new InvalidOperationException();
        _playbackAudio.Stop();
        _playbackAudio.Dispose();
    }

    private void SelectNext()
    {
        var nextTrack = Tracks.Items.SkipWhile(t => !t.Equals(SelectedTrack))
            .Skip(1)
            .DefaultIfEmpty(Tracks.Items.First())
            .FirstOrDefault();

        if (SelectedTrack != null) StopCommand.Execute().Subscribe();

        SelectedTrack = nextTrack ?? throw new NullReferenceException();
    }

    private void SelectPrevious()
    {
        var previousTrack = Tracks.Items.TakeWhile(t => !t.Equals(SelectedTrack))
            .DefaultIfEmpty(Tracks.Items.Reverse()
                .Skip(1)
                .First())
            .LastOrDefault();

        if (SelectedTrack != null) StopCommand.Execute().Subscribe();

        SelectedTrack = previousTrack ?? throw new NullReferenceException();
    }

#endregion

#region IDisposable

    private bool _disposed;

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;
        if (disposing) _playbackAudio.Dispose();

        _disposed = true;
    }

    ~TrackPlayer()
    {
        Dispose(false);
    }

#endregion
}