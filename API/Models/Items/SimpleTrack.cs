using System.ComponentModel.DataAnnotations;

namespace Api.Models.Items;

public class SimpleTrack: SimpleItemBaseDto
{
    [Required] public string Title { get; set; }
    [Required] public string ArtistName { get; set; }

    public SimpleTrack(
        string id,
        string? thumbnailUrl,
        string title, string
            artistName
    ) : base(id, thumbnailUrl)
    {
        Title = title;
        ArtistName = artistName;
    }
}