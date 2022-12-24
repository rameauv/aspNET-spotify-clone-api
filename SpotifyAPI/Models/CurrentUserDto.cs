namespace SpotifyApi.Models;

public class CurrentUserDto
{
    public CurrentUserDto(string id, string username, string name)
    {
        Id = id;
        Username = username;
        Name = name;
    }

    public string Id { get; set; }
    public string Username { get; set; }
    public string Name { get; set; }
}