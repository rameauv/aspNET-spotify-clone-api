namespace Spotify.Shared.DAL.Artist;

public interface IArtistRepository
{
    Task<Models.Artist> GetArtist(string id);
}