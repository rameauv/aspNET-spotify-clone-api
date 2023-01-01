using Spotify.Shared.BLL.Search.Models;

namespace Spotify.Shared.BLL.Search;

public interface ISearchService
{
    Task<SearchResult> SearchAsync(Models.Search search);
}