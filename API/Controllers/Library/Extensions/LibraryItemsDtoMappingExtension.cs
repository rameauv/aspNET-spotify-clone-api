using Api.Controllers.Library.Models;
using Api.Controllers.Shared.Items.Models;
using Spotify.Shared.BLL.Library.Models;

namespace Api.Controllers.Library.Extensions;

public static class LibraryItemsDtoMappingExtension
{
    public static LibraryItemsDto ToDto(this LibraryItems libraryItems)
    {
        return new LibraryItemsDto(
            libraryItems.Albums.Select(album => new LibraryItemDto<SimpleAlbumDto>(new SimpleAlbumDto(
                    album.Item.Id,
                    album.Item.ThumbnailUrl,
                    album.Item.Title,
                    album.Item.ArtistName,
                    album.Item.AlbumType
                ),
                album.LikeCreatedAt,
                album.LikeId
            )),
            libraryItems.Artists.Select(artist => new LibraryItemDto<SimpleArtistDto>(new SimpleArtistDto(
                    artist.Item.Id,
                    artist.Item.ThumbnailUrl,
                    artist.Item.Name
                ),
                artist.LikeCreatedAt,
                artist.LikeId
            )),
            libraryItems.Total,
            libraryItems.Offset,
            libraryItems.Limit
        );
    }
}