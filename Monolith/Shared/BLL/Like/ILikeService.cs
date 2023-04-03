namespace Spotify.Shared.BLL.Like;

/// <summary>
/// Provides methods for interacting with likes.
/// </summary>
public interface ILikeService
{
    /// <summary>
    /// Deletes a like.
    /// </summary>
    /// <param name="id">The ID of the like to delete.</param>
    /// <param name="userId">The ID of user associated with the like to delete.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    Task DeleteAsync(string id, string userId);
}