using Spotify.Shared.BLL.Album.Models;

namespace Spotify.Shared.BLL.Album;

/// <summary>
/// Represents a service for managing albums.
/// </summary>
public interface IAlbumService
{
    /// <summary>
    /// Retrieves an album by its ID.
    /// </summary>
    /// <param name="id">The ID of the album.</param>
    /// <returns>The album with the specified ID, or null if no such album exists.</returns>
    Task<Models.Album?> GetAsync(string id);
    /// <summary>
    /// Retrieves a list of tracks for a given album.
    /// </summary>
    /// <param name="id">The ID of the album to retrieve tracks for.</param>
    /// <param name="albumTracksRequest">A request object for specifying pagination options for the track list. If null, default values will be used.</param>
    /// <returns>An object containing the retrieved tracks and track list metadata, or null if no such album exists.</returns>
    public Task<AlbumTracks?> GetTracksAsync(string id, AlbumTracksRequest? albumTracksRequest = null);
    /// <summary>
    /// Sets a like for an album for the user identified by an access token.
    /// </summary>
    /// <param name="id">The ID of the album to like.</param>
    /// <param name="accessToken">The access token identifying the user who is liking the album.</param>
    /// <returns>The like that was set.</returns>
    /// <exception cref="Exception">Thrown if the access token does not contain a user ID.</exception>
    Task<Like.Models.Like> SetLikeAsync(string id, string accessToken);
}