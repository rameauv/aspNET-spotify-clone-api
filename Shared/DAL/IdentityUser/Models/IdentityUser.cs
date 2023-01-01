namespace Spotify.Shared.DAL.IdentityUser.Models;

public record IdentityUser(string Id, string UserName)
{
    public string Id { get; set; } = Id;

    public string UserName { get; set; } = UserName;
    
    public string? PasswordHash { get; set; }
}