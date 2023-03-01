using System.ComponentModel.DataAnnotations;

namespace Api.Controllers.Library.Models;

public class LibraryDto
{
    public LibraryDto(int likedTracksCount, LibraryItemsDto items)
    {
        LikedTracksCount = likedTracksCount;
        Items = items;
    }

    [Required] public int LikedTracksCount { get; }
    [Required] public LibraryItemsDto Items { get; }
}