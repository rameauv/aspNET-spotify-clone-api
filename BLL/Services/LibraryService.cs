using AutoMapper;
using Spotify.Shared.BLL.Library;
using Spotify.Shared.BLL.Library.Models;
using Spotify.Shared.BLL.Like.Models;
using Spotify.Shared.BLL.Models;
using Spotify.Shared.BLL.Shared;
using Spotify.Shared.BLL.Shared.Items;
using Spotify.Shared.DAL.Album;
using Spotify.Shared.DAL.Artist;
using Spotify.Shared.DAL.Like;
using Spotify.Shared.DAL.Track;
using AssociatedType = Spotify.Shared.DAL.Like.Models.AssociatedType;
using SharedDAL = Spotify.Shared.DAL;

namespace Spotify.BLL.Services;

public class LibraryService : ILibraryService
{
    private readonly ILikeRepository _likeRepository;
    private readonly IAlbumRepository _albumRepository;
    private readonly IArtistRepository _artistRepository;
    private readonly ITrackRepository _trackRepository;
    private readonly IMapper _mapper;

    public LibraryService(
        ILikeRepository likeRepository,
        IAlbumRepository albumRepository,
        IArtistRepository artistRepository,
        ITrackRepository trackRepository,
        IMapper mapper
    )
    {
        _likeRepository = likeRepository;
        _albumRepository = albumRepository;
        _artistRepository = artistRepository;
        _trackRepository = trackRepository;
        _mapper = mapper;
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
            new Shared.DAL.Like.Models.FindLikesByUserIdOptions(
                new Shared.DAL.Shared.PaginationOptions(options.Pagination.Limit, options.Pagination.Offset)
            )
            {
                AssociatedTypes = new[]
                    { AssociatedType.Album, AssociatedType.Artist }
            };
        var allLikes = await _likeRepository.FindLikesByUserId(userId, findLikesOptions);
        var itemIdLookupByAssociatedType =
            allLikes.Items.ToLookup(like => like.AssociatedType, like => like.AssociatedId);
        var likeLookupByAssociatedId = allLikes.Items.ToLookup(like => like.AssociatedId, like => like);
        var albums =
            await _albumRepository.GetAlbumsAsync(
                itemIdLookupByAssociatedType[AssociatedType.Album]);
        var artists =
            await _artistRepository.GetArtistsAsync(
                itemIdLookupByAssociatedType[AssociatedType.Artist]);

        return new LibraryItems(
            albums.Select(album =>
            {
                var like = likeLookupByAssociatedId[album.Id].First();
                return new LibraryItem<SimpleAlbum>(
                    _mapper.Map<SimpleAlbum>(album),
                    like.CreatedAt,
                    like.Id
                );
            }),
            artists.Select(artist =>
            {
                var like = likeLookupByAssociatedId[artist.Id].First();
                return new LibraryItem<SimpleArtist>(
                    _mapper.Map<SimpleArtist>(artist),
                    like.CreatedAt,
                    like.Id
                );
            }),
            allLikes.Total,
            allLikes.Offset,
            allLikes.Limit
        );
    }

    public async Task<Pagging<LibraryItem<SimpleTrack>>> FindLikedTracksAsync(string userId,
        FindLikedTracksOptions options)
    {
        var findLikeByUserIdOptions =
            new Shared.DAL.Like.Models.FindLikesByUserIdOptions(
                new Shared.DAL.Shared.PaginationOptions(
                    options.PaginationOptions.Limit,
                    options.PaginationOptions.Offset
                )
            )
            {
                AssociatedTypes = new[] { AssociatedType.Track }
            };
        var likes = await _likeRepository.FindLikesByUserId(userId, findLikeByUserIdOptions);
        var trackIds = likes.Items.Select(like => like.AssociatedId);
        var likeIdLookupByAssociatedId = likes.Items.ToLookup(like => like.AssociatedId, like => like);
        var tracks = await _trackRepository.GetTracksAsync(trackIds);
        return new Pagging<LibraryItem<SimpleTrack>>(
            tracks.Select(track =>
            {
                var like = likeIdLookupByAssociatedId[track.Id].First();
                return new LibraryItem<SimpleTrack>(
                    _mapper.Map<SimpleTrack>(track),
                    like.CreatedAt,
                    like.Id
                );
            }),
            likes.Limit,
            likes.Offset,
            likes.Total
        );
    }
}