namespace Spotify.Shared.DAL.Track;

public interface ITrackRepository
{
    Task<Models.Track> GetAsync(string id);
}