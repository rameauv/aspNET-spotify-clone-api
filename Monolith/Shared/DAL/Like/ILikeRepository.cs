namespace Spotify.Shared.DAL.Like;

public interface ILikeRepository
{
    public Task<Models.Like> SetAsync(string associatedId, string associatedType, string associatedUser);
    public Task DeleteAsync(string id, string associatedUserId);
    public Task<Models.Like?> GetByAssociatedUserIdAsync(string associatedId, string associatedUser);
    public Task<Models.FindLikesByUserIdResult> FindLikesByUserId(string userId, Models.FindLikesByUserIdOptions options);
    public Task<int> GetLikedTracksCountByUserId(string userId);
}