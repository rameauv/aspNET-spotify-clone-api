using System.ComponentModel.DataAnnotations;

namespace Api.Controllers.Shared.Items.Models;

public class SimpleItemBaseDto
{
    public SimpleItemBaseDto(string id, string? thumbnailUrl)
    {
        Id = id;
        ThumbnailUrl = thumbnailUrl;
    }

    [Required] public string Id { get; init; }
    [Required] public string? ThumbnailUrl { get; init; }
}