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
            YCoverPic pic => pic.Dir.Split('/')[2],

            _ => throw new NotSupportedException()
        };
    }

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

    private static async Task<string> DownloadCover(YCoverPic coverPic, string path)
    {
        var uri = new Uri($"{Endpoints.YandexAvatars}{coverPic.Dir}/{Endpoints.AvatarMiddleSize}");

        using var httpClient = new HttpClient();

        var imageBytes = await httpClient.GetByteArrayAsync(uri);
        await File.WriteAllBytesAsync(path, imageBytes);

        return path;
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

    public override async Task<string> LoadCoverAsync(YCover cover)
    {
        var path = BuildFilePath(CreateId(cover), CoverFileFormat);

        if (cover is not YCoverPic pic) throw new NotSupportedException();

        return CacheExists(path)
            ? path
            : await DownloadCover(pic, path);
    }

#endregion

#region Helper static methods

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
}