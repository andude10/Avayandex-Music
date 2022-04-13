using Yandex.Music.Api;
using Yandex.Music.Api.Common;

namespace Avayandex_Music.Core.Services;

public static class AuthorizationService
{
    // not done yet
    public static async Task<bool> Authorize(string login, string password)
    {
        var authStorage = new AuthStorage();
        authStorage.User.Login = login;
        
        var api = new YandexMusicApi();
        await api.User.AuthorizeAsync(authStorage, authStorage.User.Login, password);

        if (authStorage.IsAuthorized)
        {
            // await AuthStorageService.SaveAuthData(authStorage);
        }
        
        return authStorage.IsAuthorized;
    }
    
    public static bool IsAuthorized()
    {
        var authStorage = AuthStorageService.GetAuthStorage();
        return authStorage is {IsAuthorized: true};
    }
}