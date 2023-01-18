using Spotify.Shared.DAL.Track;
using Spotify.Shared.DAL.Track.Models;
using SpotifyAPI.Web;

namespace RealSpotifyDAL.Repositories;

/// <summary>
/// Repository for fetching track information from the Spotify API
/// </summary>
public class TrackRepository : ITrackRepository
{
    private readonly SpotifyClient _client;

    /// <summary>
    /// Initializes a new instance of the <see cref="TrackRepository"/> class.
    /// </summary>
    /// <param name="spotifyClient">Spotify client object</param>
    public TrackRepository(MySpotifyClient spotifyClient)
    {
        this._client = spotifyClient.SpotifyClient;
    }

    public async Task<Track?> GetAsync(string id)
    {
        try
        {
            var res = await _client.Tracks.Get(id, new TrackRequest());

            return new Track(
                res.Id,
                res.Name,
                res.Artists.FirstOrDefault()?.Name ?? "",
                res.Album.Images.FirstOrDefault()?.Url
            );
        }
        catch (APIException e)
        {
            if (e.Message == "invalid id")
            {
                return null;
            }

            throw;
        }
    }
}