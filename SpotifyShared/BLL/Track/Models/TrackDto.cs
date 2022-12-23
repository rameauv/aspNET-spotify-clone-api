namespace Spotify.Shared.BLL.Track.Models;

public record Track(string Id, string Title, string ArtistName, string? ThumbnailUrl, bool IsLiked)
{
    public string Id { get; set; } = Id;
    public string Title { get; set; } = Title;
    public string ArtistName { get; set; } = ArtistName;
    public string? ThumbnailUrl { get; set; } = ThumbnailUrl;
    public bool IsLiked { get; set; } = IsLiked;
}