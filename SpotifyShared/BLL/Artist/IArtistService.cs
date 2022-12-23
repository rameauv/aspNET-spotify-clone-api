namespace Spotify.Shared.BLL.Artist;

public interface IArtistService
{
    Task<Models.Artist> GetAsync(string id);
}