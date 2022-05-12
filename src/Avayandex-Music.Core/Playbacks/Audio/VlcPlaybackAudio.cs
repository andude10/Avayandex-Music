using LibVLCSharp.Shared;

namespace Avayandex_Music.Core.Playbacks.Audio;

public class VlcPlaybackAudio : PlaybackAudio
{
    private readonly LibVLC _libVlc;
    private readonly MediaPlayer _mediaPlayer;

    public VlcPlaybackAudio()
    {
        _libVlc = new LibVLC();
        _mediaPlayer = new MediaPlayer(_libVlc);
    }

    public VlcPlaybackAudio(string filePath) : base(filePath)
    {
        _libVlc = new LibVLC();
        _mediaPlayer = new MediaPlayer(_libVlc);

        using var media = new Media(_libVlc, filePath);
        _mediaPlayer.Media = media;
    }

#region Properties

    public override PlaybackState State { get; protected set; }

#endregion

#region PlaybackAudio

    public override void Play()
    {
        State = PlaybackState.Playing;
        _mediaPlayer.Play();
    }

    public override void Pause()
    {
        State = PlaybackState.Paused;
        _mediaPlayer.Pause();
    }

    public override void Stop()
    {
        State = PlaybackState.Stopped;
        _mediaPlayer.Stop();
    }

    public override PlaybackAudio CreatePlayback(string filePath)
    {
        return new VlcPlaybackAudio(filePath);
    }

#endregion

#region IDisposable

    private bool _disposed;

    public override void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;
        if (disposing)
        {
            _mediaPlayer.Dispose();
            _libVlc.Dispose();
        }

        _disposed = true;
    }

    ~VlcPlaybackAudio()
    {
        Dispose(false);
    }

#endregion
}