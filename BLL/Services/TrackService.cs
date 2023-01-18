using System.Security.Claims;
using Spotify.Shared.BLL.Jwt;
using Spotify.Shared.BLL.Like.Models;
using Spotify.Shared.BLL.Track;
using Spotify.Shared.BLL.Track.Models;
using Spotify.Shared.DAL.Like;
using Spotify.Shared.DAL.Track;

namespace Spotify.BLL.Services;

/// <summary>
/// A service for interacting with tracks.
/// </summary>
public class TrackService : ITrackService
{
    private readonly ITrackRepository _trackRepository;
    private readonly ILikeRepository _likeRepository;
    private readonly IJwtService _jwtService;

    /// <summary>
    /// Initializes a new instance of the <see cref="TrackService"/> class.
    /// </summary>
    /// <param name="trackRepository">The repository for interacting with tracks.</param>
    /// <param name="jwtService">The service for validating and generating JWT tokens.</param>
    /// <param name="likeRepository">The repository for interacting with likes.</param>
    public TrackService(
        ITrackRepository trackRepository,
        IJwtService jwtService,
        ILikeRepository likeRepository)
    {
        _trackRepository = trackRepository;
        this._jwtService = jwtService;
        this._likeRepository = likeRepository;
    }

    public async Task<Track?> GetAsync(string id)
    {
        var trackTask = _trackRepository.GetAsync(id);
        var likeTask = _likeRepository.GetByAssociatedIdAsync(id);
        await Task.WhenAll(trackTask, likeTask);
        var track = await trackTask;
        if (track == null)
        {
            return null;
        }

        var like = await likeTask;

        return new Track(
            track.Id,
            track.Title,
            track.ArtistName,
            track.ThumbnailUrl,
            like?.Id
        );
    }

    public async Task<Like> SetLikeAsync(string id, string accessToken)
    {
        var validatedToken = _jwtService.GetValidatedAccessToken(accessToken);
        var userId = validatedToken.Principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            throw new Exception("no userid in this access token");
        }

        var like = await _likeRepository.SetAsync(id, "track", userId);
        return new Like(like.Id, like.AssociatedId, like.AssociatedUser, like.AssociatedType);
    }
}