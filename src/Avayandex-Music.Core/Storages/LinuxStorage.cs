using System.Security.Cryptography;
using System.Text;
using Avayandex_Music.Core.API.Requests;
using Avayandex_Music.Core.Services;
using Yandex.Music.Api.Common;
using Yandex.Music.Api.Models.Common;
using Yandex.Music.Api.Models.Common.Cover;

namespace Avayandex_Music.Core.Storages;

public class LinuxStorage : Storage
{
    private const string StorageDirectory = "Cache";
    private const string TrackFileFormat = ".mp3";
    private const string CoverFileFormat = ".png";

    public LinuxStorage()
    {
        if (!Directory.Exists(StorageDirectory)) Directory.CreateDirectory(StorageDirectory);
    }

#region Public methods

    public override async Task<string> LoadTrackAsync(YTrack track)
    {
        var api = new YandexMusicApi();
        var authStorage = AuthStorageService.GetInstance();

        var path = BuildFilePath(CreateId(track), TrackFileFormat);

        return CacheExists(path)
            ? path
            : await DownloadTrack(track, api, authStorage, path);
    }

    /// <summary>
    ///     Get the path to the Cover png file.
    ///     Downloads a file if it is not in the cache.
    /// </summary>
    /// <param name="cover"></param>
    /// <returns>The path to the png file, or null if an error occurred while uploading the file</returns>
    /// <exception cref="NotSupportedException">Throws if the cover has an invalid type</exception>
    public override async Task<string?> LoadCoverAsync(YCover cover)
    {
        if (cover is YCoverError) return null;

        var path = BuildFilePath(CreateId(cover), CoverFileFormat);

        var uri = cover switch
        {
            YCoverPic pic => new Uri($"{Endpoints.YandexAvatars}{pic.Dir}/{Endpoints.AvatarMiddleSize}"),
            YCoverMosaic mosaic => new Uri($"{mosaic.ItemsUri}/{Endpoints.AvatarMiddleSize}"),
            YCoverImage image => new Uri($"{image.Uri}/{Endpoints.AvatarMiddleSize}"),
            _ => throw new NotSupportedException()
        };

        await DownloadCover(uri, path);
        return path;
    }

#endregion

#region Helper static methods

#region Download methods

    private static async Task<string> DownloadTrack(YTrack track, YandexMusicApi api, AuthStorage authStorage,
        string path)
    {
        // get download link
        var metaData = (await api.Track.GetMetadataForDownloadAsync(authStorage, track))
            .Result.First();
        var downloadInfo = await api.Track.GetDownloadFileInfoAsync(authStorage, metaData);
        var url = BuildLinkForTrackDownload(metaData, downloadInfo);

        await HttpDownloadTrackAsync(new HttpClient(), url, path);

        return path;
    }

    private static async Task<string> DownloadCover(Uri downloadUri, string path)
    {
        using var httpClient = new HttpClient();

        var imageBytes = await httpClient.GetByteArrayAsync(downloadUri);
        await File.WriteAllBytesAsync(path, imageBytes);

        return path;
    }

    private static async Task HttpDownloadTrackAsync(HttpClient httpClient, string url, string path)
    {
        using var response = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
        await using var streamToReadFrom = await response.Content.ReadAsStreamAsync();
        await using var streamToWriteTo = File.Open(path, FileMode.Create);
        await streamToReadFrom.CopyToAsync(streamToWriteTo);
    }

    private static string BuildLinkForTrackDownload(YTrackDownloadInfo mainDownloadResponse,
        YStorageDownloadFile storageDownload)
    {
        var path = storageDownload.Path;
        var host = storageDownload.Host;
        var ts = storageDownload.Ts;
        var s = storageDownload.S;
        var codec = mainDownloadResponse.Codec;

        var secret = $"XGRlBW9FXlekgbPrRHuSiA{path.Substring(1, path.Length - 1)}{s}";
        var md5 = MD5.Create();
        var md5Hash = md5.ComputeHash(Encoding.UTF8.GetBytes(secret));
        var hmacsha1 = new HMACSHA1();
        var hmasha1Hash = hmacsha1.ComputeHash(md5Hash);
        var sign = BitConverter.ToString(hmasha1Hash).Replace("-", "").ToLower();

        var link = $"https://{host}/get-{codec}/{sign}/{ts}{path}";

        return link;
    }

#endregion

#region Cache interaction methods

    private static bool CacheExists(string path)
    {
        var storageDirectory = new DirectoryInfo(StorageDirectory);
        var filesInDir = storageDirectory.GetFiles()
            .Select(file => file.Name);

        return filesInDir.Contains(new FileInfo(path).Name);
    }

    private static string BuildFilePath(string id, string fileFormat)
    {
        return Path.Combine(StorageDirectory, id) + fileFormat;
    }

    private static string CreateId(object obj)
    {
        return obj switch
        {
            YTrack track => track.Id,
            YCoverPic pic => pic.Uri.Replace('/', 'l')
                .Remove(0, pic.Uri.Length / 2)
                .Normalize(),
            YCoverMosaic mosaic => mosaic.ItemsUri.First().Replace('/', 'l')
                .Remove(0, mosaic.ItemsUri.First().Length / 2)
                .Normalize(),
            YCoverImage image => image.Uri.Replace('/', 'l')
                .Remove(0, image.Uri.Length / 2)
                .Normalize(),

            _ => throw new NotSupportedException(obj.GetType().ToString())
        };
    }

#endregion

#endregion
}