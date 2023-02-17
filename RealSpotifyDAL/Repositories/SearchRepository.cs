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

    public async Task<SearchResult> SearchAsync(SearchOptions searchOptions)
    {
        var types = MapSearchType(searchOptions.Types);
        var searchRes = await _client.Search.Item(
            new SearchRequest(
                types,
                searchOptions.Q
            )
            {
                Limit = searchOptions.Limit,
                Offset = searchOptions.Offset
            });
        var mappedAlbums = searchRes.Albums?.Items?.Select(album => new ReleaseResult(
            album.Id,
            album.Images.FirstOrDefault()?.Url,
            album.Name,
            album.Artists.FirstOrDefault()?.Name ?? ""
        )).ToArray();
        var mappedTracks = searchRes.Tracks?.Items?.Select(track => new SongResult(
            track.Id,
            track.Album.Images.FirstOrDefault()?.Url,
            track.Name,
            track.Artists.FirstOrDefault()?.Name ?? ""
        )).ToArray();
        var mappedArtists = searchRes.Artists?.Items?.Select(artist => new ArtistResult(
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

    private SearchRequest.Types MapSearchType(SearchOptions.SearchTypes? types)
    {
        if (types == null)
        {
            return SearchRequest.Types.Track | SearchRequest.Types.Album | SearchRequest.Types.Artist;
        }

        var notNullableTypes = types.Value;
        var allowedSearchTypes = new[]
            { SearchOptions.SearchTypes.Album, SearchOptions.SearchTypes.Artist, SearchOptions.SearchTypes.Track };
        return allowedSearchTypes.Aggregate<SearchOptions.SearchTypes, SearchRequest.Types>(0, (i, allowedSearchType) =>
        {
            if (notNullableTypes.HasFlag(allowedSearchType))
            {
                switch (allowedSearchType)
                {
                    case SearchOptions.SearchTypes.Album:
                        return i | SearchRequest.Types.Album;
                    case SearchOptions.SearchTypes.Artist:
                        return i | SearchRequest.Types.Artist;
                    case SearchOptions.SearchTypes.Track:
                        return i | SearchRequest.Types.Track;
                }
            }

            return i;
        });
    }
}