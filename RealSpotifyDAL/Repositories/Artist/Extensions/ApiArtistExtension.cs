using SpotifyAPI.Web;

namespace RealSpotifyDAL.Repositories.Artist.Extensions;

public static class ApiArtistExtension
{
    public static Spotify.Shared.DAL.Artist.Models.Artist ToDalArtist(this FullArtist artist)
    {
        return new Spotify.Shared.DAL.Artist.Models.Artist(
            artist.Id,
            artist.Name,
            artist.Images.FirstOrDefault()?.Url,
            artist.Followers.Total
        );
    }
}