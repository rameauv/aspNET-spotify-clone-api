namespace SpotifyApi.Models;

public class TrackDto
{
    public TrackDto(string id, string title, string artistName, string? thumbnailUrl, bool isLiked)
    {
        Id = id;
        Title = title;
        ArtistName = artistName;
        IsLiked = isLiked;
        ThumbnailUrl = thumbnailUrl;
    }

    public string Id { get; set; }
    public string Title { get; set; }
    public string ArtistName { get; set; }
    public string? ThumbnailUrl { get; set; }
    public bool IsLiked { get; set; }
}