using AutoMapper;
using Spotify.Shared.BLL.Search;
using Spotify.Shared.BLL.Search.Models;
using DAL = Spotify.Shared.DAL;

namespace Spotify.BLL.Services;

public class SearchService : ISearchService
{
    private readonly DAL.Search.ISearchRepository _searchRepository;
    private readonly IMapper _mapper;

    public SearchService(DAL.Search.ISearchRepository searchRepository, IMapper mapper)
    {
        this._searchRepository = searchRepository;
        this._mapper = mapper;
    }

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