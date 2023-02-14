using Spotify.Shared.BLL.Album.Models;

namespace Spotify.Shared.BLL.Album;

/// <summary>
/// Represents a service for managing albums.
/// </summary>
public interface IAlbumService
{
    /// <summary>
    /// Retrieves an album and its associated like, if one exists.
    /// </summary>
    /// <param name="id">The ID of the album.</param>
    /// <param name="userId">the user id associated with the like.</param>
    /// <returns>The album with the specified ID, or null if no such album exists.</returns>
    Task<Models.Album?> GetAsync(string id, string userId);

    /// <summary>
    /// Retrieves a list of tracks for a given album, if one exists.
    /// </summary>
    /// <param name="id">The ID of the album to retrieve tracks for.</param>
    /// <param name="albumTracksRequest">A request object for specifying pagination options for the track list. If null, default values will be used.</param>
    /// <returns>An object containing the retrieved tracks and track list metadata, or null if no such album exists.</returns>
    public Task<AlbumTracks?> GetTracksAsync(string id, AlbumTracksRequest? albumTracksRequest = null);

    /// <summary>
    /// Sets a like on a album with the specified album id and user id.
    /// </summary>
    /// <param name="id">The ID of the album to like.</param>
    /// <param name="userId">the user id associated with the like.</param>
    /// <returns>The like that was set.</returns>
    Task<Like.Models.Like> SetLikeAsync(string id, string userId);
}