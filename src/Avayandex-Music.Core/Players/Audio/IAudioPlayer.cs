using System.Reactive;
using ReactiveUI;

namespace Avayandex_Music.Core.Players.Audio;

public interface IAudioPlayer
{
    public enum State
    {
        Playing,
        Stopped
    }

    /// <summary>
    ///     Command to play the current audio.
    ///     CanExecute equals false, when the current audio is already playing
    /// </summary>
    public ReactiveCommand<Unit, Unit> PlayCommand { get; }

    /// <summary>
    ///     Command to play the next or first audio in the list.
    ///     CanExecute equals false, when the last audio in the list is playing
    /// </summary>
    public ReactiveCommand<Unit, Unit> PlayNextCommand { get; }

    /// <summary>
    ///     Command to play the previous audio in the list.
    ///     CanExecute is false when the last audio in the list is playing
    /// </summary>
    public ReactiveCommand<Unit, Unit> PlayPreviousCommand { get; }

    /// <summary>
    ///     Stops playing the current audio.
    ///     CanExecute is false when the audio is already stopped.
    /// </summary>
    public ReactiveCommand<Unit, Unit> StopCommand { get; }

#region Properties

    public State CurrentState { get; }
    public object CurrentStateLocker { get; }

#endregion
}