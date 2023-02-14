namespace Spotify.Shared.DAL.Like;

public interface ILikeRepository
{
    public Task<models.Like> SetAsync(string associatedId, string associatedType, string associatedUser);
    public Task DeleteAsync(string id, string associatedUserId);
    public Task<models.Like?> GetByAssociatedUserIdAsync(string associatedId, string associatedUser);
}