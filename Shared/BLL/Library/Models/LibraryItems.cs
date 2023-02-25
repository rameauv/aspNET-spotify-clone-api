using Spotify.Shared.BLL.Shared.Items;

namespace Spotify.Shared.BLL.Library.Models;

public record LibraryItems(
    IEnumerable<LibraryItem<SimpleAlbum>> Albums,
    IEnumerable<LibraryItem<SimpleArtist>> Artists,
    int Total,
    int Offset,
    int Limit
)
{
}