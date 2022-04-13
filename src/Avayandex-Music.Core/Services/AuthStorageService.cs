using System.Security.Cryptography;
using Avayandex_Music.Core.Models;
using Newtonsoft.Json;
using Avayandex_Music.Core.Security;
using Yandex.Music.Api.Common;

namespace Avayandex_Music.Core.Services;

public static class AuthStorageService
{
    private const string AuthDataFileName = "storage.txt";
    private const string AuthDataKeyFileName = "stkey.txt";
    private static AuthStorage _authStorage;
    private static object AuthStorageLocker = new object();

    // not done yet
    public static AuthStorage? GetAuthStorage()
    {
        lock (AuthStorageLocker)
        {
            if (!File.Exists(AuthDataFileName)) return null;
            if (!File.Exists(AuthDataKeyFileName)) return null;
            
            var encryptedJson = File.ReadAllText(AuthDataFileName);
            var key = File.ReadAllText(AuthDataKeyFileName);

            var json = StorageEncryption.Decrypt(encryptedJson, key);
            var authData = JsonConvert.DeserializeObject<AuthData>(json);

            return new AuthStorage();
        }
    }

    
    public static async Task SaveAuthData(AuthData authData)
    {
        await File.WriteAllTextAsync(AuthDataFileName, 
            JsonConvert.SerializeObject(authData));
        
        var json = await File.ReadAllTextAsync(AuthDataFileName);
        
        var key = CreateStorageKey();
        await File.WriteAllTextAsync(AuthDataKeyFileName, key);

        var encryptedJson = StorageEncryption.Encrypt(json, key);
        await File.WriteAllTextAsync(AuthDataFileName, encryptedJson);
    }

    private static string CreateStorageKey()
    {
        using var cryptRng = RandomNumberGenerator.Create();
        var tokenBuffer = new byte[16];
        
        cryptRng.GetBytes(tokenBuffer);
        
        var key = Convert.ToBase64String(tokenBuffer);

        return key;
    }
}