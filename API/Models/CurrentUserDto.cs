using System.ComponentModel.DataAnnotations;

namespace Api.Models;

public class CurrentUserDto
{
    public CurrentUserDto(string id, string username, string name)
    {
        Id = id;
        Username = username;
        Name = name;
    }

    [Required] public string Id { get; }
    [Required] public string Username { get; }
    [Required] public string Name { get; }
}