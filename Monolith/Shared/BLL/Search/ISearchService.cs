using Spotify.Shared.BLL.Search.Models;

namespace Spotify.Shared.BLL.Search;

/// <summary>
/// Defines methods for searching the music database.
/// </summary>
public interface ISearchService
{
    /// <summary>
    /// Searches for music.
    /// </summary>
    /// <param name="searchOptions">The search parameters.</param>
    /// <returns>The search results.</returns>
    Task<SearchResult> SearchAsync(Models.SearchOptions searchOptions);
}