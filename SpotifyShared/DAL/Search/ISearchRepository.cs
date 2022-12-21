using Spotify.Shared.DAL.Search.Models;

namespace Spotify.Shared.DAL.Search;

public interface ISearchRepository
{
    Task<SearchResult> SearchAsync(Models.Search search);
}