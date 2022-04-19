namespace Avayandex_Music.Core.Services;

public interface ILoginService
{
    /// <summary>
    ///     Login and password authorization
    /// </summary>
    /// <returns>Authorization result. True if successful, false if not/</returns>
    public Task<bool> AuthorizeAsync(string login, string password);

    /// <summary>
    ///     Authorization based on token that was saved earlier.
    /// </summary>
    /// <returns>Authorization result. True if successful, false if not.</returns>
    public Task<bool> AuthorizeAsync();
}