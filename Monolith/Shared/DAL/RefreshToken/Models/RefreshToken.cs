namespace Spotify.Shared.DAL.RefreshToken.Models;

public record RefreshToken(string Id, string UserId, string Token)
{
    public string Id { get; set; } = Id;
    public string UserId { get; set; } = UserId;
    
    public string Token { get; set; } = Token;
}