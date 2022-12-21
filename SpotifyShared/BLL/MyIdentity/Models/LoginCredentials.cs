namespace Spotify.Shared.MyIdentity.Models;

public record LoginCredentials(string Username, string Password, Guid DeviceId)
{
    public string Username { get; set; } = Username;
    public string Password { get; set; } = Password;
    
    public Guid DeviceId { get; set; } = DeviceId;
}