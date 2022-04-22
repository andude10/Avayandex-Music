using DynamicData;
using Yandex.Music.Api.Models.Track;

namespace Avayandex_Music.Core.Players.Audio.Music;

public interface IMusicPlayer : IAudioPlayer
{
#region Properties

    public SourceList<YTrack?> Tracks { get; }
    public YTrack? SelectedTrack { get; }

#endregion
}