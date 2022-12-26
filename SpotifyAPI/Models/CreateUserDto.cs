namespace SpotifyApi.Models;

public class CreateUserDto
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Data { get; set; } = null!;
}