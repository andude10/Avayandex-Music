using System.Reactive;
using Avayandex_Music.Core.Playbacks.Audio;
using Avayandex_Music.Core.Storages;
using DynamicData;
using ReactiveUI;
using Yandex.Music.Api.Models.Track;

namespace Avayandex_Music.Core.Players.Audio.Music;

public class MusicPlayer : IMusicPlayer
{
    public MusicPlayer(Storage storage, IPlaybackAudio playbackAudio)
    {
        _storage = storage;
        _playbackAudio = playbackAudio;

        var canPreviousNext = this.WhenAnyValue(
            mp => mp.Tracks.Count > 0);

        PlayCommand = ReactiveCommand.CreateFromTask(Play);
        PlayNextCommand = ReactiveCommand.CreateFromTask(PlayNext);
        PlayPreviousCommand = ReactiveCommand.CreateFromTask(PlayPrevious);
        StopCommand = ReactiveCommand.CreateFromTask(Stop);
    }

#region Fields

    private readonly Storage _storage;
    private readonly IPlaybackAudio _playbackAudio;

#endregion

#region Properties

    public IAudioPlayer.State CurrentState { get; protected set; }
    public object CurrentStateLocker { get; } = new();
    public SourceList<YTrack> Tracks { get; } = new();
    public YTrack Track { get; private set; }

#endregion

#region Commands

    public ReactiveCommand<Unit, Unit> PlayCommand { get; }

    /// <summary>
    ///     Stops playing the current track.
    ///     CanExecute is false when the track is already stopped.
    /// </summary>
    public ReactiveCommand<Unit, Unit> StopCommand { get; }

    /// <summary>
    ///     Command to play the next or first track in the list.
    ///     CanExecute equals false, when the last track in the list is playing
    /// </summary>
    public ReactiveCommand<Unit, Unit> PlayNextCommand { get; }

    /// <summary>
    ///     Command to play the previous track in the list.
    ///     CanExecute is false when the last track in the list is playing
    /// </summary>
    public ReactiveCommand<Unit, Unit> PlayPreviousCommand { get; }

#endregion

#region Methods

    private async Task Play()
    {
    }

    private async Task Stop()
    {
    }

    private async Task PlayNext()
    {
        var nextTrack = Tracks.Items.SkipWhile(t => !t.Equals(Track))
            .Skip(1)
            .DefaultIfEmpty(Tracks.Items.First())
            .FirstOrDefault();

        Track = nextTrack ?? throw new NullReferenceException();

        await Play();
    }

    private async Task PlayPrevious()
    {
        var previousTrack = Tracks.Items.TakeWhile(t => !t.Equals(Track))
            .DefaultIfEmpty(Tracks.Items.Reverse()
                .Skip(1)
                .First())
            .LastOrDefault();

        Track = previousTrack ?? throw new NullReferenceException();

        await Play();
    }

#endregion
}