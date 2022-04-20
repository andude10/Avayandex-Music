using System.Reactive;
using Avayandex_Music.Core.Playbacks;
using ReactiveUI;

namespace Avayandex_Music.Core.Players.Audio;

public interface IAudioPlayer
{
    /// <summary>
    ///     Select the audio to play
    ///     CanExecute is false when the last audio in the list is playing
    /// </summary>
    public ReactiveCommand<int, Unit> SelectCommand { get; }
    
    /// <summary>
    ///     Play the audio.
    ///     CanExecute equals false, when the current audio is already playing
    /// </summary>
    public ReactiveCommand<Unit, Unit> PlayCommand { get; }

    /// <summary>
    ///     SelectCommand the next audio to play
    ///     CanExecute equals false, when the last audio in the list is playing
    /// </summary>
    public ReactiveCommand<Unit, Unit> SelectNextCommand { get; }

    /// <summary>
    ///     SelectCommand previous the next audio to play
    ///     CanExecute is false when the last audio in the list is playing
    /// </summary>
    public ReactiveCommand<Unit, Unit> SelectPreviousCommand { get; }

    /// <summary>
    ///     Pause playing the audio.
    ///     CanExecute is false when the audio is already stopped.
    /// </summary>
    public ReactiveCommand<Unit, Unit> PauseCommand { get; }

#region Properties

    public PlaybackState State { get; }

#endregion
    
}