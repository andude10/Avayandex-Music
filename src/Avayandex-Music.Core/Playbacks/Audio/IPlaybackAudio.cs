using Avayandex_Music.Core.Storages;

namespace Avayandex_Music.Core.Playbacks.Audio;

public interface IPlaybackAudio
{
    public PlaybackState State { get; } 
        
    public void SetupAudio(string filePath);
    public void Play();
    public void Pause();
}