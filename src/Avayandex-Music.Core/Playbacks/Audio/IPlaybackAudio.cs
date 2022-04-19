namespace Avayandex_Music.Core.Playbacks.Audio;

public interface IPlaybackAudio
{
    public void SetupAudio(string audioPath);
    public void Play();
    public void Stop();
}