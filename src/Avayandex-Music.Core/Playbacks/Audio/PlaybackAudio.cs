namespace Avayandex_Music.Core.Playbacks.Audio;

/// <summary>
///     The PlaybackAudio object is responsible for
///     interacting with a single audio file (Play, Stop, State ).
/// </summary>
public abstract class PlaybackAudio : IDisposable
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

#region IDisposable

    public abstract void Dispose();

#endregion

    public abstract void Play();
    public abstract void Pause();
    public abstract void Stop();

    /// <summary>
    ///     Creates an instance of the current player
    /// </summary>
    /// <param name="filePath">Audio to play</param>
    /// <returns>New instance of the current player</returns>
    public abstract PlaybackAudio CreatePlayback(string filePath);
}