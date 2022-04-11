using Yandex.Music.Api;
using Yandex.Music.Api.Common;

namespace Avayandex_Music.Infrastructure;

// tests
public class Class1
{
    public async Task Authorize(string login, string password)
    {
        var debugSettings = new DebugSettings(@"C:\yandex_music", @"C:\yandex_music\log.txt");
        debugSettings.Clear();
        var authStorage = new AuthStorage(debugSettings);

        authStorage.User.Login = login;
        var api = new YandexMusicApi();
        await api.User.AuthorizeAsync(authStorage, authStorage.User.Login, password);
        if (authStorage.IsAuthorized)
        {
            Console.WriteLine("Успешная авторизация");
        }
        else
        {
            Console.WriteLine("Неверный логин или пароль");
        }
    }
}