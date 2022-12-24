namespace Spotify.Shared.BLL.MyIdentity.Models;

public record Token(string AccessToken, string RefreshToken)
{
    public string AccessToken { get; set; } = AccessToken;
    public string RefreshToken { get; set; } = RefreshToken;
}