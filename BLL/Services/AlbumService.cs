using System.Security.Claims;
using Spotify.Shared.BLL.Album;
using Spotify.Shared.BLL.Album.Models;
using Spotify.Shared.BLL.Jwt;
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
    private readonly IJwtService _jwtService;

    /// <summary>
    /// Initializes a new instance of the <see cref="AlbumService"/> class.
    /// </summary>
    /// <param name="albumRepository">The repository for accessing album data.</param>
    /// <param name="likeRepository">The repository for accessing like data.</param>
    /// <param name="jwtService">The service for validating and generating JWT access tokens.</param>
    public AlbumService(
        IAlbumRepository albumRepository,
        ILikeRepository likeRepository,
        IJwtService jwtService
    )
    {
        this._albumRepository = albumRepository;
        this._likeRepository = likeRepository;
        this._jwtService = jwtService;
    }

    /// <summary>
    /// Retrieves an album and its associated like, if one exists.
    /// </summary>
    /// <param name="id">The ID of the album to retrieve.</param>
    /// <returns>The retrieved album.</returns>
    public async Task<Album> GetAsync(string id)
    {
        var albumTask = _albumRepository.GetAsync(id);
        var likeTask = _likeRepository.GetByAssociatedIdAsync(id);
        await Task.WhenAll(albumTask, likeTask);
        var album = await albumTask;
        var like = await likeTask;

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

    /// <summary>
    /// Retrieves a list of tracks for a given album.
    /// </summary>
    /// <param name="id">The ID of the album to retrieve tracks for.</param>
    /// <param name="albumTracksRequest">A request object for specifying pagination options for the track list. If null, default values will be used.</param>
    /// <returns>An object containing the retrieved tracks and track list metadata.</returns>
    public async Task<AlbumTracks> GetTracksAsync(string id, AlbumTracksRequest? albumTracksRequest = null)
    {
        var res = await _albumRepository.GetTracksAsync(id, new Shared.DAL.Album.Models.AlbumTracksRequest
        {
            Limit = albumTracksRequest?.Limit,
            Offset = albumTracksRequest?.Offset
        });
        var mappedTracks = res.Items.Select(track => new SimpleTrack(
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

    /// <summary>
    /// Sets a like for an album for the user identified by an access token.
    /// </summary>
    /// <param name="id">The ID of the album to like.</param>
    /// <param name="accessToken">The access token identifying the user who is liking the album.</param>
    /// <returns>The like that was set.</returns>
    /// <exception cref="Exception">Thrown if the access token does not contain a user ID.</exception>
    public async Task<Like> SetLikeAsync(string id, string accessToken)
    {
        var validatedToken = _jwtService.GetValidatedAccessToken(accessToken);
        var userId = validatedToken.Principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            throw new Exception("no userid in this access token");
        }

        var like = await _likeRepository.SetAsync(id, "album", userId);
        return new Like(like.Id, like.AssociatedId, like.AssociatedUser, like.AssociatedType);
    }
}