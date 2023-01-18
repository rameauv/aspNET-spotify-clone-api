namespace Spotify.Shared.DAL.Track;

public interface ITrackRepository
{
    /// <summary>
    /// Retrieves an track by its ID.
    /// </summary>
    /// <param name="id">The ID of the track.</param>
    /// <returns>The track with the specified ID, or null if no such track exists.</returns>
    Task<Models.Track?> GetAsync(string id);
}