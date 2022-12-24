namespace Spotify.Shared.BLL.MyIdentity.Models;

public record MyUser(Guid Id, string UserName)
{
    public Guid Id { get; set; } = Id;
    
    public string UserName { get; set; } = UserName;

    public string? PasswordHash { get; set; }
}