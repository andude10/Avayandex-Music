using Avayandex_Music.Core.Storages;
using LibVLCSharp.Shared;

namespace Avayandex_Music.Core.Playbacks.Audio;

public class VlcPlaybackAudio : IPlaybackAudio
{
    private readonly LibVLC _libVlc;
    private readonly MediaPlayer _mediaPlayer;

    public VlcPlaybackAudio()
    {
        _libVlc = new LibVLC();
        _mediaPlayer = new MediaPlayer(_libVlc);
    }

#region Properties

    public PlaybackState State { get; private set; }

#endregion

#region IPlaybackAudio

    public void SetupAudio(string filePath)
    {
        using var media = new Media(_libVlc, filePath);
        _mediaPlayer.Media = media;
    }

    public void Play()
    {
        State = PlaybackState.Playing;
        _mediaPlayer.Play();
    }

    public void Pause()
    {
        State = PlaybackState.Stopped;
        _mediaPlayer.Pause();
    }

#endregion
    
}