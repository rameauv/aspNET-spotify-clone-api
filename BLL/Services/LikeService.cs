using Spotify.Shared.BLL.Like;
using Spotify.Shared.DAL.Like;

namespace Spotify.BLL.Services;

/// <summary>
/// A service for managing likes.
/// </summary>
public class LikeService : ILikeService
{
    private readonly ILikeRepository _likeRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="LikeService"/> class.
    /// </summary>
    /// <param name="likeRepository">The repository for storing and managing likes.</param>
    public LikeService(ILikeRepository likeRepository)
    {
        this._likeRepository = likeRepository;
    }
    
    public async Task DeleteAsync(string id)
    {
        await _likeRepository.DeleteAsync(id);
    }
}