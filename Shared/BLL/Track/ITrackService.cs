namespace Spotify.Shared.BLL.Track;

/// <summary>
/// Provides methods for interacting with track data.
/// </summary>
public interface ITrackService
{
    /// <summary>
    /// Gets a track by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the track to get.</param>
    /// <returns>The track with the specified identifier, or null if no such track exists.</returns>
    Task<Models.Track?> GetAsync(string id);

    /// <summary>
    /// Sets a like on a track with the specified identifier.
    /// </summary>
    /// <param name="id">The identifier of the track to like.</param>
    /// <param name="accessToken">The access token of the user performing the like action.</param>
    /// <returns>The created like.</returns>
    /// <exception cref="Exception">Thrown if the user id could not be extracted from the access token.</exception>
    Task<Like.Models.Like> SetLikeAsync(string id, string accessToken);
}