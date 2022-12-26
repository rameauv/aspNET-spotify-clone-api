using System.Security.Claims;
using Spotify.Shared.BLL.Artist;
using Spotify.Shared.BLL.Artist.Models;
using Spotify.Shared.BLL.Like.Models;
using Spotify.Shared.BLL.MyIdentity;
using Spotify.Shared.DAL.Artist;
using Spotify.Shared.DAL.Like;

namespace Spotify.BLL.Services;

public class ArtistService : IArtistService
{
    private readonly IArtistRepository _artistRepository;
    private readonly IMyIdentityService _identityService;
    private readonly ILikeRepository _likeRepository;

    public ArtistService(IArtistRepository artistRepository, IMyIdentityService identityService, ILikeRepository likeRepository)
    {
        this._artistRepository = artistRepository;
        this._identityService = identityService;
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
        var validatedToken = _identityService.GetSecurityToken(accessToken);
        var userId = validatedToken.Principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            throw new Exception("no userid in this access token");
        }

        var like = await _likeRepository.SetAsync(id, "artist", userId);
        return new Like(like.Id, like.AssociatedId, like.AssociatedUser, like.AssociatedType);
    }
}