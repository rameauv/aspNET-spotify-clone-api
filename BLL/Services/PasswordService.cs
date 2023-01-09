using Spotify.Shared.BLL.Password;

namespace Spotify.BLL.Services;

/// <summary>
/// A service for hashing and verifying passwords.
/// </summary>
public class PasswordService: IPasswordService
{
    /// <summary>
    /// Hashes a password.
    /// </summary>
    /// <param name="text">The password to hash.</param>
    /// <returns>The hashed password.</returns>
    public string Hash(string text)
    {
        return BCrypt.Net.BCrypt.HashPassword(text, 11, true);
    }

    /// <summary>
    /// Verifies a password.
    /// </summary>
    /// <param name="text">The password to verify.</param>
    /// <param name="hash">The hashed password to compare against.</param>
    /// <returns>True if the password is verified, false otherwise.</returns>
    public bool Verify(string text, string hash)
    {
        return BCrypt.Net.BCrypt.EnhancedVerify(text, hash);
    }
}