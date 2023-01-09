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
    /// <returns>The retrieved artist.</returns>
    Task<Models.Artist> GetAsync(string id);
    /// <summary>
    /// Sets a like for an artist for the user identified by an access token.
    /// </summary>
    /// <param name="id">The ID of the artist to like.</param>
    /// <param name="accessToken">The access token identifying the user who is liking the artist.</param>
    /// <returns>The like that was set.</returns>
    /// <exception cref="Exception">Thrown if the access token does not contain a user ID.</exception>
    Task<Like.Models.Like> SetLikeAsync(string id, string accessToken);

}