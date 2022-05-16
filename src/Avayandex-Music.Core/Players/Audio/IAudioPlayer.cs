using Avayandex_Music.Core.Playbacks;

namespace Avayandex_Music.Core.Players.Audio;

public interface IAudioPlayer
{
    /// <summary>
    ///     Command to select audio to play by its index
    ///     CanExecute is false when the last audio in the list is playing
    /// </summary>
    public ReactiveCommand<int, Unit> SelectCommand { get; }

    /// <summary>
    ///     Command to load the selected audio
    ///     (if it hasn't been loaded before) and play it.
    /// </summary>
    public ReactiveCommand<Unit, Unit> PlayAsyncCommand { get; }

    /// <summary>
    ///     Command to select next audio to play
    /// </summary>
    public ReactiveCommand<Unit, Unit> SelectNextCommand { get; }

    /// <summary>
    ///     Command to select previous audio to play
    /// </summary>
    public ReactiveCommand<Unit, Unit> SelectPreviousCommand { get; }

    /// <summary>
    ///     Command to pause playing the audio.
    ///     CanExecute is false when the audio is already stopped.
    /// </summary>
    public ReactiveCommand<Unit, Unit> PauseCommand { get; }

    /// <summary>
    ///     Command to stop playing the audio.
    ///     CanExecute is false when the audio is already stopped.
    /// </summary>
    public ReactiveCommand<Unit, Unit> StopCommand { get; }

#region Properties

    public PlaybackState State { get; }

#endregion
}