using Avayandex_Music.Core.Services;
using Yandex.Music.Api;
using Yandex.Music.Api.Models.Track;

namespace Avayandex_Music.Core.Storages;

public class FileStorage : Storage
{
    private const string StorageDirectory = "Cache";
    private const string FileFormat = ".mp3";

    public override Task<string> LoadTrackAsync(YTrack track)
    {
        return new Task<string>(() =>
        {
            var api = new YandexMusicApi();
            var fullName = Path.Combine(StorageDirectory, track.Title + FileFormat);
            api.Track.ExtractToFile(AuthStorageService.GetInstance(), track, fullName);

            return fullName;
        });
    }

    public override string? GetPath(string id)
    {
        var storageDirectory = new DirectoryInfo(StorageDirectory);
        var filesInDir = storageDirectory.GetFiles("*" + id + "*.*");

        return filesInDir.Select(foundFile => foundFile.FullName).FirstOrDefault();
    }
}