namespace Spotify.Shared.BLL.Track;

/// <summary>
/// Provides methods for interacting with track data.
/// </summary>
public interface ITrackService
{
    /// <summary>
    /// Gets a track by its identifier, if one exists..
    /// </summary>
    /// <param name="id">The identifier of the track to get.</param>
    /// <param name="userId">The associated user id.</param>
    /// <returns>The track with the specified identifier, or null if no such track exists.</returns>
    Task<Models.Track?> GetAsync(string id, string userId);

    /// <summary>
    /// Sets a like on a track with the specified track id and user id.
    /// </summary>
    /// <param name="id">The identifier of the track to like.</param>
    /// <param name="userId">The associated user id.</param>
    /// <returns>The created like.</returns>
    Task<Like.Models.Like> SetLikeAsync(string id, string userId);
}