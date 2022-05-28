using Yandex.Music.Api.Common;

namespace Avayandex_Music.Core.API.Requests;

internal class GetChartRequest : YandexWebRequest
{
    public GetChartRequest(YandexMusicApi yandex, AuthStorage auth) : base(yandex, auth)
    {
    }

    public GetChartRequest Create()
    {
        FormRequest(ApiRequestsUrls.Chart);
        
        return this;
    }
}