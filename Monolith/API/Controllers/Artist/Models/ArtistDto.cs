using System.ComponentModel.DataAnnotations;

namespace Api.Controllers.Artist.Models;

public class ArtistDto
{
    public ArtistDto(string id, string name, string? thumbnailUrl, string? likeId, int monthlyListeners)
    {
        Id = id;
        Name = name;
        ThumbnailUrl = thumbnailUrl;
        LikeId = likeId;
        MonthlyListeners = monthlyListeners;
    }

    [Required] public string Id { get; }
    [Required] public string Name { get; }
    [Required] public string? ThumbnailUrl { get; }
    [Required] public string? LikeId { get; }
    [Required] public int MonthlyListeners { get; }
}