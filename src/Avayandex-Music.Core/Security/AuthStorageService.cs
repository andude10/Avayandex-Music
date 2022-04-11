using System.Security.Cryptography;
using System.Text.Json;
using Yandex.Music.Api.Common;


namespace Avayandex_Music.Core.Security;

public static class AuthStorageService
{
    private const string StorageFileName = "storage.json";
    private const string StoragePasswordFileName = "stpswd.txt";

    public static async Task<AuthStorage?> GetAuthStorageAsync()
    {
        await using var openStream = File.OpenRead(StorageFileName);

        var authStorage = await JsonSerializer.DeserializeAsync<AuthStorage>(openStream);
        await openStream.DisposeAsync();

        return authStorage;
    }

    public static async Task SaveAuthStorageAsync(AuthStorage authStorage)
    {
        await using var createStream = File.Create(StorageFileName);
        await JsonSerializer.SerializeAsync(createStream, authStorage);
        await createStream.DisposeAsync();
        
        var json = await File.ReadAllTextAsync(StorageFileName);
        var pass = CreateStoragePassword();

        var encryptJson = StorageEncryptionService.Encrypt(json, pass);
        await File.WriteAllTextAsync(StorageFileName, encryptJson);
    }

    private static string CreateStoragePassword()
    {
        using var cryptRng = RandomNumberGenerator.Create();
        var tokenBuffer = new byte[50];
        
        cryptRng.GetBytes(tokenBuffer);
        
        var pass = Convert.ToBase64String(tokenBuffer);
        
        File.WriteAllText(StoragePasswordFileName, pass);

        return pass;
    }
}