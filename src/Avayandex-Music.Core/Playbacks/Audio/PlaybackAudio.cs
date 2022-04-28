namespace Avayandex_Music.Core.Playbacks.Audio;

/// <summary>
///     The PlaybackAudio object is responsible for
///     interacting with a single audio file (Play, Stop, State ).
/// </summary>
public abstract class PlaybackAudio
{
    public PlaybackAudio()
    {
    }

    public PlaybackAudio(string filePath)
    {
    }

    /// <summary>
    ///     Current audio state
    /// </summary>
    public abstract PlaybackState State { get; protected set; }

    public abstract void Play();
    public abstract void Pause();

    /// <summary>
    ///     Creates an instance of the current player
    /// </summary>
    /// <param name="filePath">Audio to play</param>
    /// <returns>New instance of the current player</returns>
    public abstract PlaybackAudio CreatePlayback(string filePath);
}