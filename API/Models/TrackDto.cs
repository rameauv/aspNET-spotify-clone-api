namespace Api.Models;

public class TrackDto
{
    public TrackDto(string id, string title, string artistName, string? thumbnailUrl, string? likeId)
    {
        Id = id;
        Title = title;
        ArtistName = artistName;
        LikeId = likeId;
        ThumbnailUrl = thumbnailUrl;
    }

    public string Id { get; set; }
    public string Title { get; set; }
    public string ArtistName { get; set; }
    public string? ThumbnailUrl { get; set; }
    public string? LikeId { get; set; }
}