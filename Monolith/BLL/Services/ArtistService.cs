using AutoMapper;
using Spotify.Shared.BLL.Artist;
using Spotify.Shared.BLL.Artist.Models;
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
    private readonly ILikeRepository _likeRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="ArtistService"/> class.
    /// </summary>
    /// <param name="artistRepository">The repository for accessing artist data.</param>
    /// <param name="likeRepository">The repository for accessing like data.</param>
    public ArtistService(IArtistRepository artistRepository, ILikeRepository likeRepository, IMapper mapper)
    {
        _artistRepository = artistRepository;
        _likeRepository = likeRepository;
        _mapper = mapper;
    }

    public async Task<Artist?> GetAsync(string id, string userId)
    {
        var artistTask = _artistRepository.GetAsync(id);
        var likeTask = _likeRepository.GetByAssociatedUserIdAsync(id, userId);
        await Task.WhenAll(artistTask, likeTask);
        var artist = await artistTask;

        if (artist == null)
        {
            return null;
        }

        var like = await likeTask;

        return new Artist(
            artist.Id,
            artist.Name,
            artist.ThumbnailUrl,
            like?.Id,
            artist.MonthlyListeners
        );
    }

    public async Task<Like> SetLikeAsync(string id, string userId)
    {
        var like = await _likeRepository.SetAsync(id, "artist", userId);
        var associatedType = _mapper.Map<AssociatedType>(like.AssociatedType);
        return new Like(like.Id, like.AssociatedId, like.AssociatedUser, associatedType);
    }
}