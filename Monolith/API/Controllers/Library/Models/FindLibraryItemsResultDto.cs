using System.ComponentModel.DataAnnotations;
using Api.Controllers.Shared.Items.Models;

namespace Api.Controllers.Library.Models;

public class FindLibraryItemsResultDto
{
    FindLibraryItemsResultDto(
        SimpleAlbumDto[] albumResult,
        SimpleArtistDto[] artistResult
    )
    {
        AlbumResult = albumResult;
        ArtistResult = artistResult;
    }

    [Required] public SimpleAlbumDto[] AlbumResult { get; set; }
    [Required] public SimpleArtistDto[] ArtistResult { get; set; }
}