using Spotify.Shared.DAL.Album.Models;

namespace Spotify.Shared.DAL.Album;

public interface IAlbumRepository
{
    public Task<Models.Album?> GetAsync(string id);
    public Task<AlbumTracks> GetTracksAsync(string id, AlbumTracksRequest? albumTracksRequest = null);
}