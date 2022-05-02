using System.Reactive;
using System.Reactive.Linq;
using Avayandex_Music.Core.Playbacks;
using Avayandex_Music.Core.Playbacks.Audio;
using Avayandex_Music.Core.Storages;
using DynamicData;
using ReactiveUI;
using Yandex.Music.Api.Models.Track;

namespace Avayandex_Music.Core.Players.Audio.Track;

public class TrackPlayer : ITrackPlayer
{
    public TrackPlayer(Storage storage, PlaybackAudio playbackAudio)
    {
        _storage = storage;
        _playbackAudio = playbackAudio;

        PlayAsyncCommand = ReactiveCommand.CreateFromTask(LoadAndPlayAsync);
        SelectCommand = ReactiveCommand.Create<int>(Select);

        SelectNextCommand = ReactiveCommand.Create(SelectNext,
            this.WhenAnyValue(player => player.Tracks.Items)
                .Select(x =>
                {
                    var yTracks = x.ToList();
                    return yTracks.Any() && !yTracks.Last().Equals(SelectedTrack);
                }));
        SelectPreviousCommand = ReactiveCommand.Create(SelectPrevious,
            this.WhenAnyValue(player => player.Tracks.Items)
                .Select(x =>
                {
                    var yTracks = x.ToList();
                    return yTracks.Any() && !yTracks.First().Equals(SelectedTrack);
                }));

        PauseCommand = ReactiveCommand.Create(Pause);
    }

#region Fields

    private readonly Storage _storage;
    private PlaybackAudio _playbackAudio;
    private YTrack _actualTrack = new();

#endregion

#region Properties

    public PlaybackState State => _playbackAudio.State;

    /// <summary>
    ///     All tracks in the player
    /// </summary>
    public SourceList<YTrack> Tracks { get; set; } = new();

    /// <summary>
    ///     The track selected in the player. (It plays, stops,
    ///     etc.) To select a track, use the Select* methods
    /// </summary>
    public YTrack? SelectedTrack { get; private set; }

#endregion

#region Commands

    /// <summary>
    ///     Command to select track to play by its index
    ///     CanExecute is false when the last audio in the list is playing
    /// </summary>
    public ReactiveCommand<int, Unit> SelectCommand { get; }

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
            throw new InvalidOperationException("SelectedTrack is null. Most likely, one of the Select*" +
                                                " methods was not called to select a track to play.");

        if (_actualTrack.Equals(SelectedTrack))
        {
            _playbackAudio.Play();
        }
        else
        {
            _actualTrack = SelectedTrack;

            var filePath = await _storage.LoadTrackAsync(SelectedTrack);

            _playbackAudio = _playbackAudio.CreatePlayback(filePath);
            _playbackAudio.Play();
        }
    }

    private void Pause()
    {
        if (SelectedTrack == null) throw new InvalidOperationException();
        _playbackAudio.Pause();
    }

    private void Select(int index)
    {
        SelectedTrack = Tracks.Items.ElementAt(index);
    }

    private void SelectNext()
    {
        var nextTrack = Tracks.Items.SkipWhile(t => !t.Equals(SelectedTrack))
            .Skip(1)
            .DefaultIfEmpty(Tracks.Items.First())
            .FirstOrDefault();

        SelectedTrack = nextTrack ?? throw new NullReferenceException();
    }

    private void SelectPrevious()
    {
        var previousTrack = Tracks.Items.TakeWhile(t => !t.Equals(SelectedTrack))
            .DefaultIfEmpty(Tracks.Items.Reverse()
                .Skip(1)
                .First())
            .LastOrDefault();

        SelectedTrack = previousTrack ?? throw new NullReferenceException();
    }

#endregion
}