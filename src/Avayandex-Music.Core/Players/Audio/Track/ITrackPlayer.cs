using DynamicData;

namespace Avayandex_Music.Core.Players.Audio.Track;

public interface ITrackPlayer : IAudioPlayer, IDisposable
{
#region Properties

    /// <summary>
    ///     All tracks in the player
    /// </summary>
    public SourceList<YTrack> Tracks { get; }

    /// <summary>
    ///     The track selected in the player. (It plays, stops,  etc.)
    ///     If property changes to a track that is not in the <c>Tracks</c>, then the track is added to <c>Tracks</c>
    /// </summary>
    public YTrack? SelectedTrack { get; set; }

#endregion
}