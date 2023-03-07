using System.Net;
using Microsoft.Extensions.Logging;
using Spotify.Shared.DAL.Album;
using Spotify.Shared.DAL.Album.Models;
using RealSpotify = SpotifyAPI.Web;
using SharedDAL = Spotify.Shared.DAL;

namespace RealSpotifyDAL.Repositories.Album;

/// <summary>
/// Repository for fetching album information from the Spotify API
/// </summary>
public class AlbumRepository : IAlbumRepository
{
    private readonly RealSpotify.SpotifyClient _spotifyClient;
    private readonly ILogger _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="AlbumRepository"/> class.
    /// </summary>
    /// <param name="spotifyClient">Spotify client object</param>
    /// <param name="logger">Logging service</param>
    public AlbumRepository(MySpotifyClient spotifyClient, ILogger<AlbumRepository> logger)
    {
        _spotifyClient = spotifyClient.SpotifyClient;
        _logger = logger;
    }

    public async Task<Spotify.Shared.DAL.Album.Models.Album?> TryGetAsync(string id)
    {
        try
        {
            var resAlbum = await _spotifyClient.Albums.Get(id, new RealSpotify.AlbumRequest());
            var artistId = resAlbum.Artists.FirstOrDefault()?.Id;
            if (artistId == null)
            {
                throw new Exception("Could not get the artist id for this album");
            }

            var resArtist = await _spotifyClient.Artists.Get(artistId);
            return new Spotify.Shared.DAL.Album.Models.Album(
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
        catch (RealSpotify.APIException e)
        {
            if (e.Response?.StatusCode == HttpStatusCode.BadRequest)
            {
                return null;
            }

            throw;
        }
    }

    public async Task<AlbumTracks?> TryGetTracksAsync(string id, AlbumTracksRequest? albumTracksRequest = null)
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
        catch (RealSpotify.APIException e)
        {
            if (e.Response?.StatusCode == HttpStatusCode.BadRequest)
            {
                return null;
            }

            throw;
        }
    }

    public async Task<IEnumerable<Spotify.Shared.DAL.Album.Models.Album>> GetAlbumsAsync(IEnumerable<string> albumIds)
    {
        var albumIdsList = albumIds.ToList();
        if (albumIdsList.IsEmpty())
        {
            return Array.Empty<Spotify.Shared.DAL.Album.Models.Album>();
        }

        var res = await _spotifyClient.Albums.GetSeveral(new RealSpotify.AlbumsRequest(albumIdsList));
        List<string> artistIds = res.Albums
            .Select(album => album.Artists.FirstOrDefault()?.Id)
            .Where(artistId => artistId != null)
            .ToList()!;
        var artists = await _spotifyClient.Artists.GetSeveral(new RealSpotify.ArtistsRequest(artistIds));
        var artistLookupById = artists.Artists.ToLookup(artist => artist.Id, artist => artist);

        return res.Albums
            .Where(album => album != null)
            .Select(album =>
            {
                if (album == null)
                {
                    return null;
                }

                var artistId = album.Artists.FirstOrDefault()?.Id;
                if (artistId == null)
                {
                    _logger.Log(LogLevel.Error, $"Could not find an artist for the album with id:{album.Id}");
                    return null;
                }

                var artist = artistLookupById[artistId]?.FirstOrDefault();
                if (artist == null)
                {
                    _logger.Log(LogLevel.Error, $"Could not find the artist with the id:{artistId}");
                    return null;
                }

                return new Spotify.Shared.DAL.Album.Models.Album(
                    album.Id,
                    album.Name,
                    album.ReleaseDate,
                    album.Images.FirstOrDefault()?.Url,
                    artist.Id,
                    artist.Name,
                    artist.Images.FirstOrDefault()?.Url,
                    album.AlbumType
                );
            })
            .Where(album => album != null)!;
    }
}