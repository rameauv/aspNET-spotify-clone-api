using System.ComponentModel.DataAnnotations;

namespace Api.Models.Search;

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
    [Required] public DateTime UpdateAt { get; set; }
}