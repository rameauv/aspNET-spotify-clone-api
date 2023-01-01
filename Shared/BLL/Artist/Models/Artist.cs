namespace Spotify.Shared.BLL.Artist.Models;

public record Artist(string Id, string Name, string? ThumbnailUrl, string? LikeId, int MonthlyListeners)
{
    public string Id { get; set; } = Id;
    public string Name { get; set; } = Name;
    public string? ThumbnailUrl { get; set; } = ThumbnailUrl;
    public string? LikeId { get; set; } = LikeId;
    public int MonthlyListeners { get; set; } = MonthlyListeners;
}