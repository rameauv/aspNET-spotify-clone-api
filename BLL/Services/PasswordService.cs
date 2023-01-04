using Spotify.Shared.BLL.Password;

namespace Spotify.BLL.Services;

public class PasswordService: IPasswordService
{
    public string Hash(string text)
    {
        return BCrypt.Net.BCrypt.HashPassword(text, 11, true);
    }

    public bool Verify(string text, string hash)
    {
        return BCrypt.Net.BCrypt.EnhancedVerify(text, hash);
    }
}