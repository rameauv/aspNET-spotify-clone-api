using System.ComponentModel.DataAnnotations;

namespace Api.Controllers.Search.Models;

public class ReleaseSearchResultDto : BaseSearchResultDto
{
    [Required] public string Title { get; set; }
    [Required] public string ArtistName { get; set; }

    public ReleaseSearchResultDto(string id, string? thumbnailUrl, string title, string artistName) : base(id,
        thumbnailUrl)
    {
        Title = title;
        ArtistName = artistName;
    }
}