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

    public override PlaybackAudio CreatePlayback(string filePath)
    {
        return new VlcPlaybackAudio(filePath);
    }

#endregion
}