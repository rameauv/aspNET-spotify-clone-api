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

    private class SortableResultItem : IComparable<SortableResultItem>
    {
        public SortableResultItem(BaseResult item, Action<int> callback)
        {
            Weight = item.Popularity;
            Callback = callback;
        }

        public int Weight { get; }
        public Action<int> Callback { get; }

        public int CompareTo(SortableResultItem? other)
        {
            if (other == null)
            {
                return -1;
            }

            return other.Weight - Weight;
        }
    }

    private static class SortableItemFactoryBuilder<T> where T : BaseResult
    {
        public static Func<int, SortableResultItem?> TryGet(IReadOnlyList<T> array, Action<T, int> callback)
        {
            return index =>
            {
                var item = index < array.Count ? array[index] : default;
                if (item != null)
                {
                    return new SortableResultItem(item, (order) => callback(item, order));
                }

                return null;
            };
        }

        public static IEnumerable<SortableResultItem> TryGetArray(IEnumerable<T> items, Action<T, int> callback)
        {
            return items.Select(item => { return new SortableResultItem(item, (order) => callback(item, order)); });
        }
    }

    private SearchResult OrderResults(SearchResult result, int offset)
    {
        var newTracks = new List<SongResult>();
        var newAlbums = new List<AlbumResult>();
        var newArtists = new List<ArtistResult>();

        var tracks = SortableItemFactoryBuilder<SongResult>.TryGetArray(result.SongResult, (item, order) =>
            newTracks.Add(new SongResult(item)
            {
                Order = order
            }));
        var albums = SortableItemFactoryBuilder<AlbumResult>.TryGetArray(result.AlbumResult, (item, order) =>
            newAlbums.Add(new AlbumResult(item)
            {
                Order = order
            }));
        var artists = SortableItemFactoryBuilder<ArtistResult>.TryGetArray(result.ArtistResult, (item, order) =>
            newArtists.Add(
                new ArtistResult(item)
                {
                    Order = order
                }));

        var sortableResultList = tracks.Concat(albums).Concat(artists).ToList();

        sortableResultList.Sort();
        for (int i = 0; i < sortableResultList.Count; i++)
        {
            sortableResultList[i].Callback(offset + i);
        }

        return new SearchResult(newAlbums, newTracks, newArtists);
    }
}