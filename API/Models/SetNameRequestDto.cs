using System.ComponentModel.DataAnnotations;

namespace Api.Models;

public class SetNameRequestDto
{
    public SetNameRequestDto(string name)
    {
        Name = name;
    }

    [Required]
    public string Name { get; set; }
}