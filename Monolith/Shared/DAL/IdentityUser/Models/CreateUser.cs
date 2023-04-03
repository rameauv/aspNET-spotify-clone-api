using Spotify.Shared.DAL.User.Models;

namespace Spotify.Shared.DAL.IdentityUser.Models;

public record CreateUser(string UserName, string Password, UserData data)
{
    public string UserName { get; set; } = UserName;
    public string Password { get; set; } = Password;

    public UserData Data { get; set; } = data;
}