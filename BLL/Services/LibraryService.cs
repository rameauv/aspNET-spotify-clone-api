using Spotify.Shared.BLL.Library.Models;
using Spotify.Shared.BLL.Like.Models;
using Spotify.Shared.BLL.Shared;
using SharedDAL = Spotify.Shared.DAL;

namespace Spotify.BLL.Services;

public class LibraryService
{
    private readonly SharedDAL.Like.ILikeRepository _likeRepository;
    private readonly SharedDAL.Album.IAlbumRepository _albumRepository;

    LibraryService(SharedDAL.Like.ILikeRepository likeRepository, SharedDAL.Album.IAlbumRepository albumRepository)
    {
        _likeRepository = likeRepository;
        _albumRepository = albumRepository;
    }

    public async Task<Library> GetAsync(string userId)
    {
        var likedTracksCount = await _likeRepository.GetLikedTracksCountByUserId(userId);
        await FindLibraryItemsAsync(userId, new FindLikesByUserIdOptions(new PaginationOptions(10, 0)));
        return new Library(likedTracksCount);
    }

    public async Task FindLibraryItemsAsync(string userId, FindLikesByUserIdOptions options)
    {
        var findLikesOptions =
            new SharedDAL.Like.models.FindLikesByUserIdOptions(
                new SharedDAL.Shared.PaginationOptions(options.Pagination.Limit, options.Pagination.Offset)
            )
            {
            };
        var likes = await _likeRepository.FindLikesByUserId(userId, findLikesOptions);
        var likesAssociatedIds = likes.Items.Select(like => like.AssociatedId);
        var res = await _albumRepository.GetAlbums(likesAssociatedIds);
    }

    public async Task FindLikedSongsAsync(string userId)
    {
    }
}