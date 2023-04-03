using System.ComponentModel.DataAnnotations;

namespace Api.Controllers.Search.Models;

public class ArtistSearchResultDto : BaseSearchResultDto
{
    [Required] public string Name { get; set; }

    public ArtistSearchResultDto(string id, string? thumbnailUrl, string name) : base(id, thumbnailUrl)
    {
        Name = name;
    }
}