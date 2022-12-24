namespace Spotify.Shared.DAL.IdentityUser.Models;

public record IdentityUser(Guid Id, string UserName)
{
    public Guid Id { get; set; } = Id;

    public string UserName { get; set; } = UserName;
    
    public string? PasswordHash { get; set; }
}