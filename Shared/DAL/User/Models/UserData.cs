namespace Spotify.Shared.DAL.User.Models;

public class UserData
{
    public UserData(string name)
    {
        Name = name;
    }

    public string Name { get; set; }
}