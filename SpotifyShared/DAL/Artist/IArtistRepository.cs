namespace Spotify.Shared.DAL.Artist;

public interface IArtistRepository
{
    Task<Models.Artist> GetAsync(string id);
}