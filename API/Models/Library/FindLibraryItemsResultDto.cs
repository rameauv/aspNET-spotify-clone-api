using System.ComponentModel.DataAnnotations;
using Api.Models.Items;

namespace Api.Models.Library;

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