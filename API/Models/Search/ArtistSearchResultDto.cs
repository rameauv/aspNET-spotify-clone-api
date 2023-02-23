using System.ComponentModel.DataAnnotations;

namespace Api.Models.Search;

public class ArtistSearchResultDto : BaseSearchResultDto
{
    [Required] public string Name { get; set; }

    public ArtistSearchResultDto(string id, string? thumbnailUrl, string name) : base(id, thumbnailUrl)
    {
        Name = name;
    }
}