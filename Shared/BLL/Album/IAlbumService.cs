using Spotify.Shared.BLL.Album.Models;

namespace Spotify.Shared.BLL.Album;

public interface IAlbumService
{
    Task<Models.Album> GetAsync(string id);
    public Task<AlbumTracks> GetTracksAsync(string id, AlbumTracksRequest? albumTracksRequest = null);
    Task<Like.Models.Like> SetLikeAsync(string id, string accessToken);
}