using System.Net;
using Spotify.Shared.DAL.Album;
using Spotify.Shared.DAL.Album.Models;
using SpotifyAPI.Web;
using AlbumTracksRequest = Spotify.Shared.DAL.Album.Models.AlbumTracksRequest;
using SimpleTrack = Spotify.Shared.DAL.Album.Models.SimpleTrack;
using RealSpotify = SpotifyAPI.Web;

namespace RealSpotifyDAL.Repositories;

/// <summary>
/// Repository for fetching album information from the Spotify API
/// </summary>
public class AlbumRepository : IAlbumRepository
{
    private readonly SpotifyClient _spotifyClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="AlbumRepository"/> class.
    /// </summary>
    /// <param name="spotifyClient">Spotify client object</param>
    public AlbumRepository(MySpotifyClient spotifyClient)
    {
        this._spotifyClient = spotifyClient.SpotifyClient;
    }

    public async Task<Album?> GetAsync(string id)
    {
        try
        {
            var resAlbum = await _spotifyClient.Albums.Get(id, new AlbumRequest());
            var artistId = resAlbum.Artists.FirstOrDefault()?.Id;
            if (artistId == null)
            {
                throw new Exception("could not get the artist id for this album");
            }

            var resArtist = await _spotifyClient.Artists.Get(artistId);
            return new Album(
                resAlbum.Id,
                resAlbum.Name,
                resAlbum.ReleaseDate,
                resAlbum.Images.FirstOrDefault()?.Url,
                resArtist.Id,
                resArtist.Name,
                resArtist.Images.FirstOrDefault()?.Url,
                resAlbum.AlbumType
            );
        }
        catch (APIException e)
        {
            if (e.Message == "invalid id")
            {
                return null;
            }

            throw;
        }
    }

    public async Task<AlbumTracks?> GetTracksAsync(string id, AlbumTracksRequest? albumTracksRequest = null)
    {
        try
        {
            var res = await _spotifyClient.Albums.GetTracks(id, new RealSpotify.AlbumTracksRequest()
            {
                Limit = albumTracksRequest?.Limit,
                Offset = albumTracksRequest?.Offset
            });
            if (res.Items == null)
            {
                return new AlbumTracks(Array.Empty<SimpleTrack>());
            }

            var mappedTracks = res.Items.Select(track => new SimpleTrack(
                track.Id,
                track.Name,
                track.Artists.FirstOrDefault()?.Name ?? "")
            ).ToArray();
            return new AlbumTracks(mappedTracks, res.Limit ?? 0, res.Offset ?? 0, res.Total ?? 0);
        }
        catch (APIException e)
        {
            if (e.Message == "invalid id")
            {
                return null;
            }

            throw;
        }
    }
}