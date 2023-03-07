using System.ComponentModel.DataAnnotations;

namespace Api.Controllers.Search.Models;

public class BaseSearchResultDto
{
    public BaseSearchResultDto(string id, string? thumbnailUrl)
    {
        Id = id;
        ThumbnailUrl = thumbnailUrl;
    }

    [Required] public string Id { get; set; }
    [Required] public string? ThumbnailUrl { get; set; }
    [Required] public int Order { get; set; }
}