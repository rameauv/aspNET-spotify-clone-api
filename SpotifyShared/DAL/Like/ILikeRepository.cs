using Spotify.Shared.DAL.Like.models;

namespace Spotify.Shared.DAL.Like;

public interface ILikeRepository
{
    public Task<models.Like> SetAsync(string associatedId, string associatedType, string associatedUser);
    public Task DeleteAsync(string id);
    public Task<models.Like?> GetByAssociatedIdAsync(string associatedId);
    public Task<UserLikes> GetByAssociatedUserId(string id, UserLikesRequest? userLikesRequest);
}