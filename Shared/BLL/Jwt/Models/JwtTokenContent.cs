namespace Spotify.Shared.BLL.Jwt.Models;

public class JwtTokenContent
{
    public JwtTokenContent(string userId)
    {
        UserId = userId;
    }

    public string UserId { get; set; }
}