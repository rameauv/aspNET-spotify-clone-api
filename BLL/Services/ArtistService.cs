using System.Security.Claims;
using Spotify.Shared.BLL.Artist;
using Spotify.Shared.BLL.Artist.Models;
using Spotify.Shared.BLL.Jwt;
using Spotify.Shared.BLL.Like.Models;
using Spotify.Shared.DAL.Artist;
using Spotify.Shared.DAL.Like;

namespace Spotify.BLL.Services;

/// <summary>
/// A service for performing operations on artists.
/// </summary>
public class ArtistService : IArtistService
{
    private readonly IArtistRepository _artistRepository;
    private readonly IJwtService _jwtService;
    private readonly ILikeRepository _likeRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="ArtistService"/> class.
    /// </summary>
    /// <param name="artistRepository">The repository for accessing artist data.</param>
    /// <param name="jwtService">The service for validating and generating JWT access tokens.</param>
    /// <param name="likeRepository">The repository for accessing like data.</param>
    public ArtistService(IArtistRepository artistRepository, IJwtService jwtService, ILikeRepository likeRepository)
    {
        this._artistRepository = artistRepository;
        this._jwtService = jwtService;
        this._likeRepository = likeRepository;
    }
    
    public async Task<Artist> GetAsync(string id)
    {
        var artistTask = _artistRepository.GetAsync(id);
        var likeTask = _likeRepository.GetByAssociatedIdAsync(id);
        await Task.WhenAll(artistTask, likeTask);
        var artist = await artistTask;
        var like = await likeTask;

        return new Artist(
            artist.Id,
            artist.Name,
            artist.ThumbnailUrl,
            like?.Id,
            artist.MonthlyListeners
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

        var like = await _likeRepository.SetAsync(id, "artist", userId);
        return new Like(like.Id, like.AssociatedId, like.AssociatedUser, like.AssociatedType);
    }
}