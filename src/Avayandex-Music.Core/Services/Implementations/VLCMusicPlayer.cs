using System.Reactive;
using Avayandex_Music.Core.Services.Abstractions;
using DynamicData;
using LibVLCSharp.Shared;
using ReactiveUI;
using Yandex.Music.Api;
using Yandex.Music.Api.Models.Track;

namespace Avayandex_Music.Core.Services.Implementations;

public class VlcMusicPlayer : MusicPlayer
{
    
#region Fields

    private readonly CancellationTokenSource _playingTokenSource = new CancellationTokenSource();
    private Task _playing;

#endregion

    public VlcMusicPlayer(Abstractions.MusicStorage musicStorage) : base(musicStorage)
    {
        var canPreviousNext = this.WhenAnyValue(
            mp => mp.Tracks.Count > 0);

        PlayCommand = ReactiveCommand.CreateFromTask(Play);
        PlayNextCommand = ReactiveCommand.CreateFromTask(PlayNext);
        PlayPreviousCommand = ReactiveCommand.CreateFromTask(PlayPrevious);
        StopCommand = ReactiveCommand.CreateFromTask(Stop);
    }

#region Properties

    public override State CurrentState { get; protected set; }
    public override SourceList<YTrack> Tracks { get; } = new();
    public override YTrack Track { get; protected set; }

#endregion

#region Commands

    public override ReactiveCommand<Unit, Unit> PlayCommand { get; }
    
    /// <summary>
    /// Stops playing the current track.
    /// CanExecute is false when the track is already stopped.
    /// </summary>
    public override ReactiveCommand<Unit, Unit> StopCommand { get; }

    /// <summary>
    /// Command to play the next or first track in the list.
    /// CanExecute equals false, when the last track in the list is playing
    /// </summary>
    public override ReactiveCommand<Unit, Unit> PlayNextCommand { get; }
    
    /// <summary>
    /// Command to play the previous track in the list.
    /// CanExecute is false when the last track in the list is playing
    /// </summary>
    public override ReactiveCommand<Unit, Unit> PlayPreviousCommand { get; }

#endregion

#region Methods

    private async Task Play()
    {
        /*
        if (CurrentState != State.Stopped)
        {
            _playing = new Task(() =>
            {
                var libVlc = new LibVLC();
                var mediaPlayer = new MediaPlayer(libVlc);
            
                var fileName = MusicFileStorage.GetFileName(Track) ?? throw new InvalidOperationException();
            
                mediaPlayer.Media = new Media(libVlc, fileName);

                _playingTokenSource.Token.Register(() =>
                {
                    mediaPlayer.Stop();
                });

                mediaPlayer.Play();

            }, _playingTokenSource.Token);   
        }
        */

        await _playing;
    }
    
    private async Task Stop()
    {
        _playingTokenSource.Cancel();

        if (_playingTokenSource.Token.IsCancellationRequested)
        {
            CurrentState = State.Stopped;
        }

        await Play();
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