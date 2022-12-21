namespace Spotify.Shared.BLL.Release;

public interface IReleaseService
{
    Task<Models.Release> GetAsync(string id);
    Task FindByArtistAsync(string id);
}