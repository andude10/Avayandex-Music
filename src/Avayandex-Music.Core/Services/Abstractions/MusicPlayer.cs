using System.Reactive;
using DynamicData;
using ReactiveUI;
using Yandex.Music.Api.Models.Track;

namespace Avayandex_Music.Core.Services.Abstractions;

public abstract class MusicPlayer : ReactiveObject
{
    protected readonly MusicStorage MusicStorage;
    
    public MusicPlayer(MusicStorage musicStorage)
    {
        MusicStorage = musicStorage;
    }
    
    public enum State
    {
        Playing,
        Stopped
    }

    /// <summary>
    /// Command to play the current track.
    /// CanExecute equals false, when the current track is already playing
    /// </summary>
    public abstract ReactiveCommand<Unit, Unit> PlayCommand { get; }
    
    /// <summary>
    /// Command to play the next or first track in the list.
    /// CanExecute equals false, when the last track in the list is playing
    /// </summary>
    public abstract ReactiveCommand<Unit, Unit> PlayNextCommand { get; }
    
    /// <summary>
    /// Command to play the previous track in the list.
    /// CanExecute is false when the last track in the list is playing
    /// </summary>
    public abstract ReactiveCommand<Unit, Unit> PlayPreviousCommand { get; }
    
    /// <summary>
    /// Stops playing the current track.
    /// CanExecute is false when the track is already stopped.
    /// </summary>
    public abstract ReactiveCommand<Unit, Unit> StopCommand { get; }

#region Properties

#region Lockers

    public object CurrentStateLocker = new object();

#endregion
    
    public abstract State CurrentState { get; protected set; }
    public abstract SourceList<YTrack> Tracks { get; }
    public abstract YTrack Track { get; protected set; }

#endregion

}