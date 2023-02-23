using System.ComponentModel.DataAnnotations;

namespace Api.Models.Library;

public class SongLibraryItem: BaseLibraryItemDto
{
    [Required] public string Title { get; set; }
    [Required] public string ArtistName { get; set; }

    public SongLibraryItem(
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