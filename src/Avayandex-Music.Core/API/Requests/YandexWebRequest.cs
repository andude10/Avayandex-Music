using System.Net;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using Yandex.Music.Api.Common;
using Yandex.Music.Api.Models.Common;

namespace Avayandex_Music.Core.API.Requests;

/// <summary>
/// Class representing a web request to Yandex api
/// </summary>
internal class YandexWebRequest
{
    private readonly YandexMusicApi _api;

    private HttpWebRequest _fullRequest;
    private readonly AuthStorage _storage;

    public YandexWebRequest(YandexMusicApi yandex, AuthStorage auth)
    {
        _api = yandex;
        _storage = auth;
    }

    private string GetQueryString(Dictionary<string, string> query)
    {
        return string.Join("&", query.Select(p => $"{p.Key}={HttpUtility.UrlEncode(p.Value)}"));
    }

    protected virtual void FormRequest(string url, string method = WebRequestMethods.Http.Get,
        Dictionary<string, string>? query = null, List<KeyValuePair<string, string>>? headers = null, string? body = null)
    {
        var queryStr = string.Empty;
        if (query is {Count: > 0})
            queryStr = "?" + GetQueryString(query);

        var uri = new Uri($"{url}{queryStr}");
        var request = WebRequest.CreateHttp(uri);

        if (_storage.Context.WebProxy != null)
            request.Proxy = _storage.Context.WebProxy;

        request.Method = method;
        _storage.Context.Cookies ??= new CookieContainer();

        _storage.SetHeaders(request);

        if (headers is {Count: > 0})
            foreach (var header in headers)
                request.Headers.Add(header.Key, header.Value);

        if (!string.IsNullOrEmpty(body))
        {
            var bytes = Encoding.UTF8.GetBytes(body);
            var s = request.GetRequestStream();
            s.Write(bytes, 0, bytes.Length);

            request.ContentLength = bytes.Length;
        }

        request.CookieContainer = _storage.Context.Cookies;
        request.KeepAlive = true;
        request.Headers[HttpRequestHeader.AcceptCharset] = Encoding.UTF8.WebName;
        request.Headers[HttpRequestHeader.AcceptEncoding] = "gzip";
        request.AutomaticDecompression = DecompressionMethods.GZip;

        _fullRequest = request;
    }

    private async Task<T?> GetDataFromResponseAsync<T>(HttpWebResponse response)
    {
        try
        {
            string result;
            await using (var stream = response.GetResponseStream())
            {
                var reader = new StreamReader(stream);
                result = await reader.ReadToEndAsync();
            }

            _storage.Context.Cookies.Add(response.Cookies);

            var settings = new JsonSerializerSettings
            {
                Converters = new List<JsonConverter>
                {
                    new YExecutionContextConverter(_api, _storage)
                }
            };

            if (_storage.Debug != null)
                return _storage.Debug.Deserialize<T>(response.ResponseUri.AbsolutePath, result, settings);

            return JsonConvert.DeserializeObject<T>(result, settings);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return default;
        }
    }

    private async Task<HttpWebResponse> GetResponseAsync()
    {
        try
        {
            return (HttpWebResponse) await _fullRequest.GetResponseAsync();
        }
        catch (Exception ex)
        {
            using var sr = new StreamReader(((WebException) ex).Response?.GetResponseStream() ?? throw new InvalidOperationException());
            var result = await sr.ReadToEndAsync();
            Console.WriteLine(result);

            throw;
        }
    }

    public async Task<T?> GetResponseAsync<T>()
    {
        using var response = await GetResponseAsync();
        return await GetDataFromResponseAsync<T>(response);
    }
}