using Avayandex_Music.Core.Services.Abstractions;
using Yandex.Music.Api;
using Yandex.Music.Api.Models.Track;

namespace Avayandex_Music.Core.Services.Implementations;

public class MusicFileStorage : Abstractions.MusicStorage
{
    private const string StorageDirectory = "Cache";
    private const string FileFormat = ".mp3";
    
    public override Task<string> LoadAsync(YTrack track) =>
        new Task<string>(() =>
        {
            var api = new YandexMusicApi();
            var fileName = CreateFileName(track);
            api.Track.ExtractToFile(AuthStorageService.GetInstance(), track, fileName);

            return fileName;
        });

    public override string? GetFileName(YTrack track)
    {
        var fileName = CreateFileName(track);
        return File.Exists(fileName) ? fileName : null;
    }

    private static string CreateFileName(YTrack track)
    {
        return track.Title + FileFormat;
    }
}