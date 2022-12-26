using System.ComponentModel.DataAnnotations;

namespace SpotifyApi.Models;

public class SetNameRequestDto
{
    public SetNameRequestDto(string name)
    {
        Name = name;
    }

    [Required]
    public string Name { get; set; }
}