using Spotify.Shared.DAL.Search;
using Spotify.Shared.DAL.Search.Models;
using SpotifyAPI.Web;

namespace RealSpotifyDAL.Repositories;

public class SearchRepository : ISearchRepository
{
    private readonly SpotifyClient _client;

    public SearchRepository(MySpotifyClient client)
    {
        this._client = client.SpotifyClient;
    }

    public async Task<SearchResult> SearchAsync(Search search)
    {
        var searchRes = await _client.Search.Item(
            new SearchRequest(
                SpotifyAPI.Web.SearchRequest.Types.Album | SpotifyAPI.Web.SearchRequest.Types.Artist |
                SpotifyAPI.Web.SearchRequest.Types.Track,
                search.Query
            )
            {
                Limit = search.Limit,
                Offset = search.Offset
            });
        var mappedAlbums = searchRes.Albums.Items?.Select(album => new ReleaseResult(
            album.Id,
            album.Images.FirstOrDefault()?.Url,
            album.Name,
            album.Artists.FirstOrDefault()?.Name ?? ""
        )).ToArray();
        var mappedTracks = searchRes.Tracks.Items?.Select(track => new SongResult(
            track.Id,
            track.Album.Images.FirstOrDefault()?.Url,
            track.Name,
            track.Artists.FirstOrDefault()?.Name ?? ""
        )).ToArray();
        var mappedArtists = searchRes.Artists.Items?.Select(artist => new ArtistResult(
            artist.Id,
            artist.Images.FirstOrDefault()?.Url,
            artist.Name
        )).ToArray();
        return new SearchResult(
            mappedAlbums ?? Array.Empty<ReleaseResult>(),
            mappedTracks ?? Array.Empty<SongResult>(),
            mappedArtists ?? Array.Empty<ArtistResult>()
        );
    }
}