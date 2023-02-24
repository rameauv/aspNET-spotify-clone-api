using RealSpotifyDAL.Repositories.Artist.Extensions;
using Spotify.Shared.DAL.Artist;
using SpotifyAPI.Web;

namespace RealSpotifyDAL.Repositories.Artist;

/// <summary>
/// Repository for fetching artist information from the Spotify API
/// </summary>
public class ArtistRepository : IArtistRepository
{
    private readonly SpotifyClient _client;

    /// <summary>
    /// Initializes a new instance of the <see cref="ArtistRepository"/> class.
    /// </summary>
    /// <param name="spotifyClient">Spotify client object</param>
    public ArtistRepository(MySpotifyClient spotifyClient)
    {
        this._client = spotifyClient.SpotifyClient;
    }

    public async Task<Spotify.Shared.DAL.Artist.Models.Artist?> GetAsync(string id)
    {
        try
        {
            var res = await _client.Artists.Get(id);

            return res.ToDalArtist();
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

    public async Task<IEnumerable<Spotify.Shared.DAL.Artist.Models.Artist>> GetArtistsAsync(IEnumerable<string> artistsIds)
    {
        var res = await _client.Artists.GetSeveral(new ArtistsRequest(artistsIds.ToList()));
        return res.Artists.Select(artist => artist.ToDalArtist());
    }
}