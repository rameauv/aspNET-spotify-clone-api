using System.ComponentModel.DataAnnotations;

namespace SpotifyApi.Models;

public class CurrentUserDto
{
    public CurrentUserDto(string id, string username, string name)
    {
        Id = id;
        Username = username;
        Name = name;
    }

    [Required] public string Id { get; set; }
    [Required] public string Username { get; set; }
    [Required] public string Name { get; set; }
}