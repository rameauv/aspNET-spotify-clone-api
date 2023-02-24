using Spotify.Shared.BLL.Album.Models;
using Spotify.Shared.BLL.Artist.Models;
using Spotify.Shared.BLL.Library;
using Spotify.Shared.BLL.Library.Models;
using Spotify.Shared.BLL.Like.Models;
using Spotify.Shared.BLL.Models;
using Spotify.Shared.BLL.Shared;
using Spotify.Shared.BLL.Track.Models;
using Spotify.Shared.DAL.Track;
using SharedDAL = Spotify.Shared.DAL;

namespace Spotify.BLL.Services;

public class LibraryService : ILibraryService
{
    private readonly SharedDAL.Like.ILikeRepository _likeRepository;
    private readonly SharedDAL.Album.IAlbumRepository _albumRepository;
    private readonly SharedDAL.Artist.IArtistRepository _artistRepository;
    private readonly ITrackRepository _trackRepository;

    LibraryService(
        SharedDAL.Like.ILikeRepository likeRepository,
        SharedDAL.Album.IAlbumRepository albumRepository,
        SharedDAL.Artist.IArtistRepository artistRepository,
        SharedDAL.Track.ITrackRepository trackRepository
    )
    {
        _likeRepository = likeRepository;
        _albumRepository = albumRepository;
        _artistRepository = artistRepository;
        _trackRepository = trackRepository;
    }

    public async Task<Library> GetAsync(string userId)
    {
        var likedTracksCount = await _likeRepository.GetLikedTracksCountByUserId(userId);
        var items = await FindLibraryItemsAsync(userId, new FindLikesByUserIdOptions(new PaginationOptions(10, 0)));
        return new Library(likedTracksCount, items);
    }

    public async Task<LibraryItems> FindLibraryItemsAsync(string userId, FindLikesByUserIdOptions options)
    {
        var findLikesOptions =
            new SharedDAL.Like.Models.FindLikesByUserIdOptions(
                new SharedDAL.Shared.PaginationOptions(options.Pagination.Limit, options.Pagination.Offset)
            )
            {
                AssociatedTypes = new[]
                    { SharedDAL.Like.Models.AssociatedType.Album, SharedDAL.Like.Models.AssociatedType.Artist }
            };
        var allLikes = await _likeRepository.FindLikesByUserId(userId, findLikesOptions);
        var likesLookupByAssociatedType =
            allLikes.Items.ToLookup(like => like.AssociatedType, like => like.AssociatedId);
        var likeIdLookupByAssociatedId = allLikes.Items.ToLookup(like => like.AssociatedId, like => like.Id);
        var albums =
            await _albumRepository.GetAlbumsAsync(
                likesLookupByAssociatedType[SharedDAL.Like.Models.AssociatedType.Album]);
        var artists =
            await _artistRepository.GetArtistsAsync(
                likesLookupByAssociatedType[SharedDAL.Like.Models.AssociatedType.Artist]);

        return new LibraryItems(
            albums.Select(album => new Album(
                album.Id,
                album.Title,
                album.ReleaseDate,
                album.ThumbnailUrl,
                album.ArtistId,
                album.ArtistName,
                album.ArtistThumbnailUrl,
                album.AlbumType,
                likeIdLookupByAssociatedId[album.Id].FirstOrDefault()
            )),
            artists.Select(artist => new Artist(
                artist.Id,
                artist.Name,
                artist.ThumbnailUrl,
                likeIdLookupByAssociatedId[artist.Id].FirstOrDefault(),
                artist.MonthlyListeners
            )),
            allLikes.Total,
            allLikes.Offset,
            allLikes.Limit
        );
    }

    public async Task<Pagging<Track>> FindLikedTracksAsync(string userId, FindLikedTracksOptions options)
    {
        var findLikeByUserIdOptions =
            new Shared.DAL.Like.Models.FindLikesByUserIdOptions(
                new Shared.DAL.Shared.PaginationOptions(
                    options.PaginationOptions.Limit,
                    options.PaginationOptions.Offset
                )
            )
            {
                AssociatedTypes = new[] { SharedDAL.Like.Models.AssociatedType.Track }
            };
        var likes = await _likeRepository.FindLikesByUserId(userId, findLikeByUserIdOptions);
        var likeIds = likes.Items.Select(like => like.Id);
        var likeIdLookupByAssociatedId = likes.Items.ToLookup(like => like.AssociatedId, like => like.Id);
        var tracks = await _trackRepository.GetTracksAsync(likeIds);
        return new Pagging<Track>(
            tracks.Select(track => new Track(
                track.Id,
                track.Title,
                track.ArtistName,
                track.ThumbnailUrl,
                likeIdLookupByAssociatedId[track.Id].FirstOrDefault()
            )),
            likes.Limit,
            likes.Offset,
            likes.Total
        );
    }
}