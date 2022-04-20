using Yandex.Music.Api.Models.Track;

namespace Avayandex_Music.Core.Storages;

public abstract class Storage
{
    /// <summary>
    ///     Loads track into the storage and returns the full file name
    /// </summary>
    /// <returns></returns>
    public abstract Task<string> LoadTrack(YTrack? track);

    /// <summary>
    ///     If the file with this id is in storage, returns its full file name
    /// </summary>
    /// <returns>Full file name, or null if the file is not in the storage</returns>
    public abstract string? GetPath(string id);
}