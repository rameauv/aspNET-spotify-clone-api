namespace Spotify.Shared.DAL.Artist;

/// <summary>
/// Repository for fetching artist information
/// </summary>
public interface IArtistRepository
{
    /// <summary>
    /// Retrieves an artist by its ID.
    /// </summary>
    /// <param name="id">The ID of the artist.</param>
    /// <returns>The artist with the specified ID, or null if no such artist exists.</returns>
    Task<Models.Artist?> GetAsync(string id);

    Task<IEnumerable<Models.Artist>> GetArtistsAsync(IEnumerable<string> artistsIds);
}