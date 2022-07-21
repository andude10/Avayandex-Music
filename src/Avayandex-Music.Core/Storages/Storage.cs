using Yandex.Music.Api.Models.Common.Cover;

namespace Avayandex_Music.Core.Storages;

/// <summary>
///     The Storage Object is responsible for loading, retrieving and clearing the cache.
/// </summary>
/*
 * The user accesses the storage in order
 * to get an external file (for example, a track)
 * and if the file has not been loaded before, the storage loads it.
 */
public abstract class Storage
{
    /// <summary>
    ///     Loads track into the storage and returns the full file name
    /// </summary>
    /// <returns></returns>
    public abstract Task<string> LoadTrackAsync(YTrack track);

    /// <summary>
    ///     Get the path to the Cover png file.
    ///     Downloads a file if it is not in the cache.
    /// </summary>
    /// <param name="cover"></param>
    /// <returns>The path to the png file, or null if an error occurred while uploading the file</returns>
    /// <exception cref="NotSupportedException">Throws if the cover has an invalid type</exception>
    public abstract Task<string?> LoadCoverAsync(YCover cover);
}