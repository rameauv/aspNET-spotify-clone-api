namespace Spotify.Shared.BLL.User.Models;

public record User(string Id, string Username, string Name)
{
    public string Id { get; set; } = Id;
    public string Username { get; set; } = Username;
    public string Name { get; set; } = Name;
}