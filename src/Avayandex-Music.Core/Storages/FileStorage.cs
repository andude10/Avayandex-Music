using System.Net;
using System.Security.Cryptography;
using System.Text;
using Avayandex_Music.Core.Services;
using Yandex.Music.Api;
using Yandex.Music.Api.Models.Common;
using Yandex.Music.Api.Models.Track;

namespace Avayandex_Music.Core.Storages;

public class FileStorage : Storage
{
    private const string StorageDirectory = "Cache";
    private const string FileFormat = ".mp3";

    public override async Task<string> LoadTrack(YTrack? track)
    {
        var api = new YandexMusicApi();
        var authStorage = AuthStorageService.GetInstance();
        
        // get download link
        var metaData = (await api.Track.GetMetadataForDownloadAsync(authStorage, track))
            .Result.First();
        var downloadInfo = await api.Track.GetDownloadFileInfoAsync(authStorage, metaData);
        var url = BuildLinkForDownload(metaData, downloadInfo);
        
        // download track from link
        var path = Path.Combine(StorageDirectory, track.Id + FileFormat);
        await HttpDownloadFileAsync(new HttpClient(), url, path);

        return path;
    }

    public override string? GetPath(string id)
    {
        if (!Directory.Exists(StorageDirectory)) Directory.CreateDirectory(StorageDirectory);

        var storageDirectory = new DirectoryInfo(StorageDirectory);
        var filesInDir = storageDirectory.GetFiles("*" + id + "*.*");

        return filesInDir.Select(foundFile => foundFile.FullName).FirstOrDefault();
    }
    
    private static async Task HttpDownloadFileAsync(HttpClient httpClient, string url, string path) 
    {
        using var response = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
        await using var streamToReadFrom = await response.Content.ReadAsStreamAsync();
        await using var streamToWriteTo = File.Open(path, FileMode.Create); 
        await streamToReadFrom.CopyToAsync(streamToWriteTo);
    }

    private static string BuildLinkForDownload(YTrackDownloadInfo mainDownloadResponse, YStorageDownloadFile storageDownload)
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
}