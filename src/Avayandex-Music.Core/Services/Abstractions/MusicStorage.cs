using Yandex.Music.Api.Models.Track;

namespace Avayandex_Music.Core.Services.Abstractions;

public abstract class MusicStorage
{
    /// <summary>
    /// Loads into the track storage
    /// and returns the track's fully qualified filename
    /// </summary>
    /// <returns></returns>
    public abstract Task<string> LoadAsync(YTrack track);
    
    /// <summary>
    /// If the track is in storage, returns its full name
    /// </summary>
    /// <returns>File name, or null if the track is not in the storage</returns>
    public abstract string? GetFileName(YTrack track);
}