namespace Spotify.Shared.DAL.RefreshToken.Models;

public record CreateRefreshToken(string UserId, string Token)
{
    public string UserId { get; set; } = UserId;
    public string Token { get; set; } = Token;
}