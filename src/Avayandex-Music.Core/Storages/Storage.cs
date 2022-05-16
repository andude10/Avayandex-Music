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
}