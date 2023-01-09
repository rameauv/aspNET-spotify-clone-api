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
    /// <param name="search">The search parameters.</param>
    /// <returns>The search results.</returns>
    Task<SearchResult> SearchAsync(Models.Search search);
}