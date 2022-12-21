namespace SpotifyApi.Models;

public record CreateUserDto
{
    public string username { get; set; } = null!;
    public string password { get; set; } = null!;
    public string data { get; set; } = null!;
}