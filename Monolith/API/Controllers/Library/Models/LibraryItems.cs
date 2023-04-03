using System.ComponentModel.DataAnnotations;
using Api.Controllers.Shared.Items.Models;

namespace Api.Controllers.Library.Models;

public class LibraryItemsDto
{
    public LibraryItemsDto(
        IEnumerable<LibraryItemDto<SimpleAlbumDto>> albums,
        IEnumerable<LibraryItemDto<SimpleArtistDto>> artists,
        int total,
        int offset,
        int limit
    )
    {
        Albums = albums;
        Artists = artists;
        Total = total;
        Offset = offset;
        Limit = limit;
    }

    [Required] public IEnumerable<LibraryItemDto<SimpleAlbumDto>> Albums { get; set; }
    [Required] public IEnumerable<LibraryItemDto<SimpleArtistDto>> Artists { get; set; }
    [Required] public int Total { get; set; }
    [Required] public int Offset { get; set; }
    [Required] public int Limit { get; set; }
}