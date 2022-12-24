namespace Spotify.Shared.DAL.IdentityUser.Models;

public record CreateUser(string UserName, string Password)
{
    public string UserName { get; set; } = UserName;
    public string Password { get; set; } = Password;
}