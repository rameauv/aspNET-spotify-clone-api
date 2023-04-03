using Spotify.Shared.BLL.Search.Models;

namespace Spotify.Shared.BLL.Search.tools;

public static class SearchOptionExtensions
{
    public static DAL.Search.Models.SearchOptions ToDalSearchOptions(this SearchOptions searchOptions)
    {
        return new DAL.Search.Models.SearchOptions(searchOptions.Q)
        {
            Limit = searchOptions.Limit,
            Offset = searchOptions.Offset,
            Types = searchOptions.Types?.ToDalSearchTypes()
        };
    }
}