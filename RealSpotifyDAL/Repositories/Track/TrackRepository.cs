using RealSpotifyDAL.Repositories.Track.Extensions;
using Spotify.Shared.DAL.Track;
using SpotifyAPI.Web;

namespace RealSpotifyDAL.Repositories.Track;

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

    public async Task<Spotify.Shared.DAL.Track.Models.Track?> GetAsync(string id)
    {
        try
        {
            var res = await _client.Tracks.Get(id, new TrackRequest());

            return new Spotify.Shared.DAL.Track.Models.Track(
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

    public async Task<IEnumerable<Spotify.Shared.DAL.Track.Models.Track>> GetTracksAsync(IEnumerable<string> trackIds)
    {
        var res = await _client.Tracks.GetSeveral(new TracksRequest(trackIds.ToList()));
        return res.Tracks.Select(track => track.ToDalTrack());
    }
}