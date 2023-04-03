using System.ComponentModel.DataAnnotations;

namespace Api.Controllers.Shared.Items.Models;

public class SimpleTrackDto : SimpleItemBaseDto
{
    [Required] public string Title { get; init; }
    [Required] public string ArtistName { get; init; }

    public SimpleTrackDto(
        string id,
        string? thumbnailUrl,
        string title,
        string artistName
    ) : base(id, thumbnailUrl)
    {
        Title = title;
        ArtistName = artistName;
    }
}