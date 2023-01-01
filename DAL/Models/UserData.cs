namespace Repositories.Models;

public class UserData
{
    public UserData(string name)
    {
        Name = name;
    }

    public string Name { get; set; }
}