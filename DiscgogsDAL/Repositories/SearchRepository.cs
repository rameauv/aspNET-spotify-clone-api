using System.Net.Http.Headers;
using System.Net.Http.Json;
using AutoMapper;
using DiscgogsDAL.models;
using Flurl;
using Spotify.Shared.DAL.Search;
using Spotify.Shared.DAL.Search.Models;

namespace DiscgogsDAL.Repositories;

public class SearchRepository : BaseRepository, ISearchRepository
{
    private readonly IMapper _mapper;

    public SearchRepository(IMapper mapper) : base()
    {
        this._mapper = mapper;
    }

    public async Task<SearchResult?> SearchAsync(Search search)
    {
        var queryParameters = new
        {
            q = search.Query,
            key = _key,
            secret = _secret,
            page = search.Page,
            per_page = search.PerPage
        };

        var url = _basePath
            .AppendPathSegment("database/search")
            .SetQueryParams(queryParameters)
            .ToString();
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        var productValue = new ProductInfoHeaderValue("SpotifyClone", "1.0");
        var commentValue = new ProductInfoHeaderValue("(+https://github.com/rameauv/spotify-ionic-react-clone)");

        request.Headers.UserAgent.Add(productValue);
        request.Headers.UserAgent.Add(commentValue);

        var res = await _client.SendAsync(request);
        if (!res.IsSuccessStatusCode)
        {
            await Console.Error.WriteLineAsync(res.StatusCode + " " + res.ReasonPhrase);
            return null;
        }

        var discogsSearchResponse = await res.Content.ReadFromJsonAsync<DiscogsSearchResponse>();
        if (discogsSearchResponse == null)
        {
            await Console.Error.WriteLineAsync("could not deserialize");
            return null;
        }

        var pagination = new SearchPagination
        {
            PerPage = discogsSearchResponse.Pagination.PerPage,
            Items = discogsSearchResponse.Pagination.Items,
            Page = discogsSearchResponse.Pagination.Page,
            Pages = discogsSearchResponse.Pagination.Pages
        };
        var searchResult = new SearchResult
        {
            Pagination = pagination
        };
        return searchResult;
    }
}