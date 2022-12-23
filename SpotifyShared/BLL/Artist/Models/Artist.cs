namespace Spotify.Shared.BLL.Artist.Models;

public record Artist(string Id, string Name, string? ThumbnailUrl, bool IsFollowing, int MonthlyListeners)
{
    public string Id { get; set; } = Id;
    public string Name { get; set; } = Name;
    public string? ThumbnailUrl { get; set; } = ThumbnailUrl;
    public bool IsFollowing { get; set; } = IsFollowing;
    public int MonthlyListeners { get; set; } = MonthlyListeners;
}