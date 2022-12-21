namespace Spotify.Shared.DAL;

public record CreateUser(string UserName, string Password)
{
    public string UserName { get; set; } = UserName;
    public string Password { get; set; } = Password;
}