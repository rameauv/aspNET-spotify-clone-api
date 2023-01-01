using Spotify.Shared.BLL.Like;
using Spotify.Shared.DAL.Like;

namespace Spotify.BLL.Services;

public class LikeService : ILikeService
{
    private ILikeRepository _likeRepository;

    public LikeService(ILikeRepository likeRepository)
    {
        this._likeRepository = likeRepository;
    }

    public async Task DeleteAsync(string id)
    {
        await _likeRepository.DeleteAsync(id);
    }
}