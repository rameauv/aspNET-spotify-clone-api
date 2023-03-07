using AutoMapper;
using Spotify.Shared.BLL.Album;
using Spotify.Shared.BLL.Album.Models;
using Spotify.Shared.BLL.Like.Models;
using Spotify.Shared.DAL.Album;
using Spotify.Shared.DAL.Like;

namespace Spotify.BLL.Services;

/// <summary>
/// A service for performing operations on albums.
/// </summary>
public class AlbumService : IAlbumService
{
    private readonly IAlbumRepository _albumRepository;
    private readonly ILikeRepository _likeRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="AlbumService"/> class.
    /// </summary>
    /// <param name="albumRepository">The repository for accessing album data.</param>
    /// <param name="likeRepository">The repository for accessing like data.</param>
    /// <param name="mapper">The object mapper service</param>
    public AlbumService(
        IAlbumRepository albumRepository,
        ILikeRepository likeRepository,
        IMapper mapper
    )
    {
        _albumRepository = albumRepository;
        _likeRepository = likeRepository;
        _mapper = mapper;
    }

    public async Task<Album?> GetAsync(string id, string userId)
    {
        var albumTask = _albumRepository.TryGetAsync(id);
        var likeTask = _likeRepository.GetByAssociatedUserIdAsync(id, userId);
        await Task.WhenAll(albumTask, likeTask);
        var album = await albumTask;
        var like = await likeTask;

        if (album == null)
        {
            return null;
        }

        return new Album(
            album.Id,
            album.Title,
            album.ReleaseDate,
            album.ThumbnailUrl,
            album.ArtistId,
            album.ArtistName,
            album.ArtistThumbnailUrl,
            album.AlbumType,
            like?.Id
        );
    }

    public async Task<AlbumTracks?> GetTracksAsync(string id, AlbumTracksRequest? albumTracksRequest = null)
    {
        var res = await _albumRepository.TryGetTracksAsync(id, new Shared.DAL.Album.Models.AlbumTracksRequest
        {
            Limit = albumTracksRequest?.Limit,
            Offset = albumTracksRequest?.Offset
        });

        if (res == null)
        {
            return null;
        }

        var mappedTracks = res.Items.Select(track => new AlbumTrack(
            track.Id,
            track.Title,
            track.ArtistName
        )).ToArray();
        return new AlbumTracks(
            mappedTracks,
            res.Limit,
            res.Offset,
            res.Total
        );
    }

    public async Task<Like> SetLikeAsync(string id, string userId)
    {
        var like = await _likeRepository.SetAsync(id, "album", userId);
        var associatedType = _mapper.Map<AssociatedType>(like.AssociatedType);
        return new Like(like.Id, like.AssociatedId, like.AssociatedUser, associatedType);
    }
}