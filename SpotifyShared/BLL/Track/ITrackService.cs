namespace Spotify.Shared.BLL.Track;

public interface ITrackService
{
    Task<Models.Track> GetAsync(string id);
}