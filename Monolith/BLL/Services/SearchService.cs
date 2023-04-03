using AutoMapper;
using Spotify.Shared.BLL.Search;
using Spotify.Shared.BLL.Search.Models;
using Spotify.Shared.BLL.Search.tools;
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
        _searchRepository = searchRepository;
        _mapper = mapper;
    }

    public async Task<SearchResult> SearchAsync(SearchOptions searchOptions)
    {
        var dalSearchOptions = searchOptions.ToDalSearchOptions();
        var res = await _searchRepository.SearchAsync(dalSearchOptions);
        return _mapper.Map<SearchResult>(res);
    }
}