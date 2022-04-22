using Yandex.Music.Api.Common;

namespace Avayandex_Music.Core.Services;

public static class AuthStorageService
{
    private static readonly AuthStorage AuthStorage = new();
    private static readonly object AuthStorageLocker = new();

    public static AuthStorage GetInstance()
    {
        lock (AuthStorageLocker)
        {
            return AuthStorage;
        }
    }
}