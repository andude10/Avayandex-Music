using DynamicData;
using Yandex.Music.Api.Models.Track;

namespace Avayandex_Music.Core.Players.Audio.Track;

public interface ITrackPlayer : IAudioPlayer
{
#region Properties

    /// <summary>
    ///     All tracks in the player
    /// </summary>
    public SourceList<YTrack> Tracks { get; set; }

    /// <summary>
    ///     The track selected in the player. (It plays, stops,
    ///     etc.) To select a track, use the Select* methods
    /// </summary>
    public YTrack? SelectedTrack { get; }

#endregion
}