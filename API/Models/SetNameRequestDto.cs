using System.ComponentModel.DataAnnotations;

namespace Api.Models;

public class SetNameRequestDto
{
    public SetNameRequestDto(string name)
    {
        Name = name;
    }

    public string Name { get; }
}