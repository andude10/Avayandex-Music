using Avayandex_Music.Core.Security;

namespace Avayandex_Music.Core.Services;

public class LoginService : ILoginService
{
    private const string TokenFileName = "tken.txt";
    private const string AuthDataKeyFileName = "stkey.txt";

    /// <summary>
    ///     Login and password authorization
    /// </summary>
    /// <returns>Authorization result. True if successful, false if not/</returns>
    public async Task<bool> AuthorizeAsync(string login, string password)
    {
        var authStorage = AuthStorageService.GetInstance();
        authStorage.User.Login = login;

        var api = new YandexMusicApi();
        await api.User.AuthorizeAsync(authStorage, authStorage.User.Login, password);

        if (authStorage.IsAuthorized) await SaveTokenAsync(authStorage.Token);

        return authStorage.IsAuthorized;
    }

    /// <summary>
    ///     Authorization based on token that was saved earlier.
    /// </summary>
    /// <returns>Authorization result. True if successful, false if not.</returns>
    public async Task<bool> AuthorizeAsync()
    {
        if (!File.Exists(TokenFileName) &&
            !File.Exists(AuthDataKeyFileName)) return false;

        // Get data for authentication
        var key = File.ReadAllText(AuthDataKeyFileName);
        var token = AuthStorageEncryption.Decrypt(File.ReadAllText(TokenFileName), key);

        if (token == null)
            throw new NullReferenceException("Token is null. Most likely, "
                                             + "the problem arose during decryption");

        // Authentication
        var authStorage = AuthStorageService.GetInstance();

        var api = new YandexMusicApi();
        await api.User.AuthorizeAsync(authStorage, token);

        return authStorage.IsAuthorized;
    }

    private async Task SaveTokenAsync(string token)
    {
        var key = AuthStorageEncryption.CreateStorageKey();
        await File.WriteAllTextAsync(AuthDataKeyFileName, key);

        var encryptedToken = AuthStorageEncryption.Encrypt(token, key);
        await File.WriteAllTextAsync(TokenFileName, encryptedToken);
    }
}