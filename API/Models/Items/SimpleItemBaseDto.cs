using System.ComponentModel.DataAnnotations;

namespace Api.Models.Items;

public class SimpleItemBaseDto
{
    public SimpleItemBaseDto(string id, string? thumbnailUrl)
    {
        Id = id;
        ThumbnailUrl = thumbnailUrl;
    }

    [Required] public string Id { get; set; }
    [Required] public string? ThumbnailUrl { get; set; }
    [Required] public DateTime UpdateAt { get; set; }
}