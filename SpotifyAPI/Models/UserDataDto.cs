namespace SpotifyApi.Models;

public class UserDataDto
{
    public UserDataDto(string name)
    {
        Name = name;
    }

    public string Name { get; set; }
}