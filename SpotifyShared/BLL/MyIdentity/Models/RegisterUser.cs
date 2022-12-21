namespace Spotify.Shared.MyIdentity.Models;

public record RegisterUser(string Username, string Password, string Data)
{
   public string Username { get; set; } = Username;
   public string Password { get; set; } = Password;
   public string Data { get; set; } = Data;
}