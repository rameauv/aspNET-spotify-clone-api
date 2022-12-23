namespace SpotifyApi.Models;

public class SimpleTrackDto
{
    public SimpleTrackDto(string id, string title, string artistName)
    {
        Id = id;
        Title = title;
        ArtistName = artistName;
    }

    public string Id { get; set; }
    public string Title { get; set; }
    public string ArtistName { get; set; }
}