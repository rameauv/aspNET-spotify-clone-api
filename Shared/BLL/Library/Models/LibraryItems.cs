namespace Spotify.Shared.BLL.Library.Models;

public record LibraryItems(
    IEnumerable<Album.Models.Album> Albums,
    IEnumerable<Artist.Models.Artist> Artists,
    int Total,
    int Offset,
    int Limit
)
{
}