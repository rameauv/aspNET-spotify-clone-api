using Spotify.Shared.BLL.Album;
using Spotify.Shared.BLL.Album.Models;
using Spotify.Shared.DAL.Album;

namespace Spotify.BLL.Services;

public class AlbumService : IAlbumService
{
    private readonly IAlbumRepository _albumRepository;

    public AlbumService(IAlbumRepository albumRepository)
    {
        this._albumRepository = albumRepository;
    }

    public async Task<Album> GetAsync(string id)
    {
        var res = await _albumRepository.GetAsync(id);
        return new Album(
            res.Id,
            res.Title,
            res.ReleaseDate,
            res.ThumbnailUrl,
            res.ArtistId,
            res.ArtistName,
            res.ArtistThumbnailUrl,
            res.AlbumType,
            true
        );
    }

    public async Task<AlbumTracks> GetTracksAsync(string id, AlbumTracksRequest? albumTracksRequest = null)
    {
        var res = await _albumRepository.GetTracksAsync(id, new Shared.DAL.Album.Models.AlbumTracksRequest
        {
            Limit = albumTracksRequest?.Limit,
            Offset = albumTracksRequest?.Offset
        });
        var mappedTracks = res.Items.Select(track => new SimpleTrack(
            track.Id,
            track.Title,
            track.ArtistName
        )).ToArray();
        return new AlbumTracks(
            mappedTracks,
            res.Limit,
            res.Offset,
            res.Total
        );
    }
}