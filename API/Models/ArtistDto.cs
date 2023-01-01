using System.ComponentModel.DataAnnotations;

namespace Api.Models;

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

    [Required] public string Id { get; set; }
    public string Name { get; set; }
    public string? ThumbnailUrl { get; set; }
    public string? LikeId { get; set; }

    public int MonthlyListeners { get; set; }
}