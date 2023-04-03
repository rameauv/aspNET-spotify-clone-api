using System.ComponentModel.DataAnnotations;

namespace Api.Controllers.Shared.Items.Models;

public class SimpleArtistDto : SimpleItemBaseDto
{
    [Required] public string Name { get; set; }

    public SimpleArtistDto(string id, string? thumbnailUrl, string name)
        : base(id, thumbnailUrl)
    {
        Name = name;
    }
}