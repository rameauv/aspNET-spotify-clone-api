using System.ComponentModel.DataAnnotations;

namespace Api.Models.Library;

public class TrackLibraryItem: BaseLibraryItemDto
{
    [Required] public string Title { get; set; }
    [Required] public string ArtistName { get; set; }

    public TrackLibraryItem(
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