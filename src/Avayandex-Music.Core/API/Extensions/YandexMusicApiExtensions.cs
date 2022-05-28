using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Avayandex_Music.Core.API.Models;
using Avayandex_Music.Core.API.Requests;
using Yandex.Music.Api.Common;
using Yandex.Music.Api.Models.Common;

namespace Avayandex_Music.Core.API.Extensions;

/// <summary>
/// Extensions for the YandexMusicApi class from the library K1llMan/Yandex.Music.Api
/// </summary>
public static class YandexMusicApiExtensions
{
    public static async Task<YResponse<Chart>?> GetChartAsync(this YandexMusicApi api, AuthStorage storage)
    {
        return await new GetChartRequest(api, storage)
            .Create()
            .GetResponseAsync<YResponse<Chart>>();
    }
}