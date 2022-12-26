namespace Spotify.Shared.BLL.Track;

public interface ITrackService
{
    Task<Models.Track> GetAsync(string id);

    Task<Like.Models.Like> SetLikeAsync(string id, string accessToken);

}