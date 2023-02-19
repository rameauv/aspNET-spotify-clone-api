using System.Collections.Immutable;
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
        var mappedAlbums = searchRes.Albums?.Items?.Select(album => new AlbumResult(
            album.Id,
            album.Images.FirstOrDefault()?.Url,
            album.Name,
            album.Artists.FirstOrDefault()?.Name ?? "",
            0
        ));
        var mappedTracks = searchRes.Tracks?.Items?.Select(track => new SongResult(
            track.Id,
            track.Album.Images.FirstOrDefault()?.Url,
            track.Name,
            track.Artists.FirstOrDefault()?.Name ?? "",
            track.Popularity
        ));
        var mappedArtists = searchRes.Artists?.Items?.Select(artist => new ArtistResult(
            artist.Id,
            artist.Images.FirstOrDefault()?.Url,
            artist.Name,
            artist.Popularity
        ));
        return OrderResults(
            new SearchResult(
                mappedAlbums ?? Array.Empty<AlbumResult>(),
                mappedTracks ?? Array.Empty<SongResult>(),
                mappedArtists ?? Array.Empty<ArtistResult>()
            ),
            searchOptions.Offset ?? 0
        );
    }

    private class SortableResultItem : IComparable<SortableResultItem>
    {
        private readonly int _weight;

        public SortableResultItem(BaseResult item, Action<int> callback)
        {
            _weight = item.Popularity;
            Callback = callback;
        }

        public Action<int> Callback { get; }

        public int CompareTo(SortableResultItem? other)
        {
            if (other == null)
            {
                return -1;
            }

            return other._weight - _weight;
        }
    }

    private static class SortableItemFactoryProvider<T> where T : BaseResult
    {
        public static IEnumerable<SortableResultItem> Get(IEnumerable<T> items, Action<T, int> callback)
        {
            return items.Select(item => { return new SortableResultItem(item, (order) => callback(item, order)); });
        }
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
        return allowedSearchTypes.Aggregate<SearchOptions.SearchTypes, SearchRequest.Types>(0,
            (acc, allowedSearchType) =>
            {
                if (notNullableTypes.HasFlag(allowedSearchType))
                {
                    switch (allowedSearchType)
                    {
                        case SearchOptions.SearchTypes.Album:
                            return acc | SearchRequest.Types.Album;
                        case SearchOptions.SearchTypes.Artist:
                            return acc | SearchRequest.Types.Artist;
                        case SearchOptions.SearchTypes.Track:
                            return acc | SearchRequest.Types.Track;
                    }
                }

                return acc;
            });
    }

    /// <summary>
    /// Orders the search result items according to their order and returns a new search result object.
    /// </summary>
    /// <param name="result">The original search result object.</param>
    /// <param name="offset">The offset to use when computing the order of the search result items.</param>
    /// <returns>A new search result object with ordered items.</returns>
    private static SearchResult OrderResults(SearchResult result, int offset)
    {
        var listCount = 3;
        var newTracks = new List<SongResult>();
        var newAlbums = new List<AlbumResult>();
        var newArtists = new List<ArtistResult>();

        var tracks = SortableItemFactoryProvider<SongResult>.Get(result.SongResult, (item, order) =>
            newTracks.Add(new SongResult(item)
            {
                Order = order
            }));
        var albums = SortableItemFactoryProvider<AlbumResult>.Get(result.AlbumResult, (item, order) =>
            newAlbums.Add(new AlbumResult(item)
            {
                Order = order
            }));
        var artists = SortableItemFactoryProvider<ArtistResult>.Get(result.ArtistResult, (item, order) =>
            newArtists.Add(
                new ArtistResult(item)
                {
                    Order = order
                }));

        var computedOffset = offset * listCount;
        var sortableResultList = tracks.Concat(albums).Concat(artists).ToList();
        sortableResultList.Sort();
        for (int i = 0; i < sortableResultList.Count; i++)
        {
            sortableResultList[i].Callback(computedOffset + i);
        }

        return new SearchResult(newAlbums, newTracks, newArtists);
    }
}