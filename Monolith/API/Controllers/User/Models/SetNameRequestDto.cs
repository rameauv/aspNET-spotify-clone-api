namespace Api.Controllers.User.Models;

public class SetNameRequestDto
{
    public SetNameRequestDto(string name)
    {
        Name = name;
    }

    public string Name { get; }
}