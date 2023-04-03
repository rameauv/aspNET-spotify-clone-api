namespace Spotify.Shared.BLL.Artist;

/// <summary>
/// Provides methods for interacting with artists.
/// </summary>
public interface IArtistService
{
    /// <summary>
    /// Retrieves an artist and its associated like, if one exists.
    /// </summary>
    /// <param name="id">The ID of the artist to retrieve.</param>
    /// <param name="userId">The associated userId.</param>
    /// <returns>The retrieved artist, or null if no such artist exists.</returns>
    Task<Models.Artist?> GetAsync(string id, string userId);

    /// <summary>
    /// Sets a like on a artist with the specified artist id and user id.
    /// </summary>
    /// <param name="id">The ID of the artist to like.</param>
    /// <param name="userId">The associated userId.</param>
    /// <returns>The like that was set.</returns>
    Task<Like.Models.Like> SetLikeAsync(string id, string userId);
}