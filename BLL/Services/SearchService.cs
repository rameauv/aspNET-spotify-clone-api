using AutoMapper;
using Spotify.Shared.BLL.Search;
using Spotify.Shared.BLL.Search.Models;
using DAL = Spotify.Shared.DAL;

namespace Spotify.BLL.Services;

/// <summary>
/// A service for searching for music.
/// </summary>
public class SearchService : ISearchService
{
    private readonly DAL.Search.ISearchRepository _searchRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="SearchService"/> class.
    /// </summary>
    /// <param name="searchRepository">The repository for searching for music.</param>
    /// <param name="mapper">The object mapper to use for mapping between types.</param>
    public SearchService(DAL.Search.ISearchRepository searchRepository, IMapper mapper)
    {
        this._searchRepository = searchRepository;
        this._mapper = mapper;
    }

    /// <summary>
    /// Searches for music.
    /// </summary>
    /// <param name="search">The search parameters.</param>
    /// <returns>The search results.</returns>
    public async Task<SearchResult> SearchAsync(Search search)
    {
        var res = await _searchRepository.SearchAsync(new DAL.Search.Models.Search(
            search.Query,
            search.Offset,
            search.Limit
        ));
        return _mapper.Map<SearchResult>(res);
    }
}