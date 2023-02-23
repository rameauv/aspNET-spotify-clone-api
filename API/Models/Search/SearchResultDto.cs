using System.ComponentModel.DataAnnotations;

namespace Api.Models.Search;

public record SearchResultDto(
    ReleaseSearchResultDto[] AlbumResult,
    SongSearchResultDto[] SongResult,
    ArtistSearchResultDto[] ArtistResult
)
{
    [Required] public ReleaseSearchResultDto[] AlbumResult { get; set; } = AlbumResult;
    [Required] public SongSearchResultDto[] SongResult { get; set; } = SongResult;
    [Required] public ArtistSearchResultDto[] ArtistResult { get; set; } = ArtistResult;
}