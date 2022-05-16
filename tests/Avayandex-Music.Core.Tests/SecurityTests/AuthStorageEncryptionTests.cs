using Avayandex_Music.Core.Security;

namespace Avayandex_Music.Core.Tests.SecurityTests;

public class AuthStorageEncryptionTests
{
    [Fact]
    public void Compare_plain_text_and_encrypted()
    {
        // Arrange
        const string plainText = "123!@#$%^&*()lsqwer";

        // Act
        var encryptedText = AuthStorageEncryption.Encrypt(plainText, "123");

        // Assert
        Assert.True(plainText != encryptedText);
    }

    [Fact]
    public void Compare_original_text_and_decrypted()
    {
        // Arrange
        const string originalText = "123!@#$%^&*()lsqwer";
        const string key = "123";

        // Act
        var decryptedText = AuthStorageEncryption.Decrypt(
            AuthStorageEncryption.Encrypt(originalText, key), key);

        // Assert
        Assert.True(originalText == decryptedText);
    }
}