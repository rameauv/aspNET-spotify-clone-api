namespace Spotify.Shared.BLL.Password;

/// <summary>
/// Provides methods for hashing and verifying passwords.
/// </summary>
public interface IPasswordService
{
    /// <summary>
    /// Hashes a password.
    /// </summary>
    /// <param name="text">The password to hash.</param>
    /// <returns>The hashed password.</returns>
    public string Hash(string text);

    /// <summary>
    /// Verifies a password.
    /// </summary>
    /// <param name="text">The password to verify.</param>
    /// <param name="hash">The hashed password to compare against.</param>
    /// <returns>True if the password is verified, false otherwise.</returns>
    public bool Verify(string text, string hash);
}