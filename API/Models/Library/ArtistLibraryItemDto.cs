using System.ComponentModel.DataAnnotations;

namespace Api.Models.Library;

public class ArtistLibraryItemDto : BaseLibraryItemDto
{
    [Required] public string Name { get; set; }

    public ArtistLibraryItemDto(string id, string? thumbnailUrl, string name) : base(id, thumbnailUrl)
    {
        Name = name;
    }
}