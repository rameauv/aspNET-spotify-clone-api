namespace Api.Models.Library;

public record LibraryItemsDto(
    IEnumerable<Spotify.Shared.BLL.Album.Models.Album> Albums,
    IEnumerable<Spotify.Shared.BLL.Artist.Models.Artist> Artists,
    int Total,
    int Offset,
    int Limit
)
{
}