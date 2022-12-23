using Spotify.Shared.DAL.Track;
using Spotify.Shared.DAL.Track.Models;
using SpotifyAPI.Web;

namespace RealSpotifyDAL.Repositories;

public class TrackRepository : ITrackRepository
{
    private readonly SpotifyClient _client;

    public TrackRepository(MySpotifyClient client)
    {
        this._client = client.SpotifyClient;
    }

    public async Task<Track> GetAsync(string id)
    {
        var res = await _client.Tracks.Get(id, new TrackRequest());

        return new Track(
            res.Id,
            res.Name,
            res.Artists.FirstOrDefault()?.Name ?? "",
            res.Album.Images.FirstOrDefault()?.Url
        );
    }
}