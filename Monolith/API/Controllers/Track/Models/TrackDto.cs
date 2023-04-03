using System.ComponentModel.DataAnnotations;

namespace Api.Controllers.Track.Models;

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

    [Required]
    public string Id { get; }
    [Required]
    public string Title { get; }
    [Required]
    public string ArtistName { get; }
    [Required]
    public string? ThumbnailUrl { get; }
    [Required]
    public string? LikeId { get; }
}