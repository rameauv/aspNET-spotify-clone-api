using System.ComponentModel.DataAnnotations;

namespace Api.Models.Library;

public class FindLibraryItemsResultDto
{
    FindLibraryItemsResultDto(
        AlbumLibraryItemDto[] albumResult,
        ArtistLibraryItemDto[] artistResult
    )
    {
        AlbumResult = albumResult;
        ArtistResult = artistResult;
    }

    [Required] public AlbumLibraryItemDto[] AlbumResult { get; set; }
    [Required] public ArtistLibraryItemDto[] ArtistResult { get; set; }
}