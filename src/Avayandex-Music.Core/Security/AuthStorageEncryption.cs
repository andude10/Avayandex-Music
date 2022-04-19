using System.Security.Cryptography;
using System.Text;

namespace Avayandex_Music.Core.Security;

public static class AuthStorageEncryption
{
    public static string Encrypt(string plainText, string key)
    {
        var clearBytes = Encoding.Unicode.GetBytes(plainText);

        using var encryptor = Aes.Create();

        var pdb = new Rfc2898DeriveBytes(key,
            new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76});
        encryptor.Key = pdb.GetBytes(32);
        encryptor.IV = pdb.GetBytes(16);

        using var ms = new MemoryStream();

        using (var cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
        {
            cs.Write(clearBytes, 0, clearBytes.Length);
            cs.Close();
        }

        plainText = Convert.ToBase64String(ms.ToArray());

        return plainText;
    }


    public static string Decrypt(string cipherText, string key)
    {
        cipherText = cipherText.Replace(" ", "+");
        var cipherBytes = Convert.FromBase64String(cipherText);

        using var encryptor = Aes.Create();

        var pdb = new Rfc2898DeriveBytes(key,
            new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76});
        encryptor.Key = pdb.GetBytes(32);
        encryptor.IV = pdb.GetBytes(16);

        using var ms = new MemoryStream();

        using (var cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
        {
            cs.Write(cipherBytes, 0, cipherBytes.Length);
            cs.Close();
        }

        cipherText = Encoding.Unicode.GetString(ms.ToArray());

        return cipherText;
    }

    public static string CreateStorageKey()
    {
        using var cryptRng = RandomNumberGenerator.Create();
        var tokenBuffer = new byte[16];

        cryptRng.GetBytes(tokenBuffer);

        var key = Convert.ToBase64String(tokenBuffer);

        return key;
    }
}