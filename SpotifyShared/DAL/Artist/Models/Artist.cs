namespace Spotify.Shared.DAL.Artist.Models;

public class Artist
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Thumbnail { get; set; }
    public bool IsFollowing { get; set; }
    public int MonthlyListeners { get; set; }
}