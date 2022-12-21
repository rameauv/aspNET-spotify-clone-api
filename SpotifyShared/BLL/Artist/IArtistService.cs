namespace Spotify.Shared.BLL.Artist;

public interface IArtistService
{
    Task<Models.Artist> GetArtist(string id);
}