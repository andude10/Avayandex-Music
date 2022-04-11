using Avayandex_Music.Core.Models;
using Yandex.Music.Api;
using Yandex.Music.Api.Common;

namespace Avayandex_Music.Core.Security;

public static class AuthViewModel
{
    public static async Task<bool> Authorize(AuthRequest request)
    {
        var authStorage = await AuthStorageService.GetAuthStorageAsync();
        
        authStorage.User.Login = request.Login;
        var api = new YandexMusicApi();
        await api.User.AuthorizeAsync(authStorage, authStorage.User.Login, request.Password);

        return authStorage.IsAuthorized;
    }
}