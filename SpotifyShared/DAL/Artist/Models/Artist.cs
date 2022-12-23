namespace Spotify.Shared.DAL.Artist.Models;

public record Artist(string Id, string Name, string? ThumbnailUrl, int MonthlyListeners)
{
    public string Id { get; set; } = Id;
    public string Name { get; set; } = Name;
    public string? ThumbnailUrl { get; set; } = ThumbnailUrl;
    public int MonthlyListeners { get; set; } = MonthlyListeners;
}