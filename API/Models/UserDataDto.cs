using System.ComponentModel.DataAnnotations;

namespace Api.Models;

public class UserDataDto
{
    public UserDataDto(string name)
    {
        Name = name;
    }

    [Required]
    public string Name { get; }
}