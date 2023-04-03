using SpotifyAPI.Web;

namespace RealSpotifyDAL.Repositories.Track.Extensions;

public static class ApiTrackExtension
{
    public static Spotify.Shared.DAL.Track.Models.Track ToDalTrack(this FullTrack track)
    {
        return new Spotify.Shared.DAL.Track.Models.Track(
            track.Id,
            track.Name,
            track.Artists.FirstOrDefault()?.Name ?? "",
            track.Album.Images.FirstOrDefault()?.Url
        );
    }
}