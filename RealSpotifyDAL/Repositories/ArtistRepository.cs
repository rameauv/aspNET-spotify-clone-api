using Spotify.Shared.DAL.Artist;
using Spotify.Shared.DAL.Artist.Models;
using SpotifyAPI.Web;

namespace RealSpotifyDAL.Repositories;

public class ArtistRepository : IArtistRepository
{
    private readonly SpotifyClient _client;

    public ArtistRepository(MySpotifyClient spotifyClient)
    {
        this._client = spotifyClient.SpotifyClient;
    }

    public async Task<Artist?> GetAsync(string id)
    {
        try
        {
            var res = await _client.Artists.Get(id);

            return new Artist(
                res.Id,
                res.Name,
                res.Images.FirstOrDefault()?.Url,
                res.Followers.Total
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