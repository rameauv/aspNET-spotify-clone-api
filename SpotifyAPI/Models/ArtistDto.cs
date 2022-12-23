namespace SpotifyApi.Models;

public class ArtistDto
{
    public ArtistDto(string id, string name, string? thumbnailUrl, bool isFollowing, int monthlyListeners)
    {
        Id = id;
        Name = name;
        ThumbnailUrl = thumbnailUrl;
        IsFollowing = isFollowing;
        MonthlyListeners = monthlyListeners;
    }

    public string Id { get; set; }
    public string Name { get; set; }
    public string? ThumbnailUrl { get; set; }
    public bool IsFollowing { get; set; }
    public int MonthlyListeners { get; set; }
}