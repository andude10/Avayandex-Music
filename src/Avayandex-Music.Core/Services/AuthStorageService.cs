using Yandex.Music.Api.Common;

namespace Avayandex_Music.Core.Services;

public static class AuthStorageService
{
    private static readonly AuthStorage AuthStorage = new AuthStorage();
    private static readonly object AuthStorageLocker = new object();

    public static AuthStorage GetInstance()
    {
        lock (AuthStorageLocker)
        {
            return AuthStorage;
        }
    }
}