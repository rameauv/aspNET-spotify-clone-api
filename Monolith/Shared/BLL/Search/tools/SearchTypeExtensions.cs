using Spotify.Shared.BLL.Search.Models;

namespace Spotify.Shared.BLL.Search.tools;

public static class SearchTypeExtensions
{
    public static DAL.Search.Models.SearchOptions.SearchTypes ToDalSearchTypes(this SearchOptions.SearchTypes types)
    {
        var allowedBllSearchTypes = Enum.GetValues<SearchOptions.SearchTypes>();
        var convertedType =
            allowedBllSearchTypes.Aggregate<SearchOptions.SearchTypes, DAL.Search.Models.SearchOptions.SearchTypes?>(
                null,
                (acc, allowedBllSearchType) =>
                {
                    if (!types.HasFlag(allowedBllSearchType))
                    {
                        return acc;
                    }

                    var convertedType = BllSearchTypeToDalSearchType(allowedBllSearchType);

                    if (acc == null)
                    {
                        return convertedType;
                    }

                    return acc | convertedType;
                });
        if (convertedType == null)
        {
            throw new InvalidOperationException("could not convert this bll search types to a dal search types");
        }

        return convertedType.Value;
    }

    private static DAL.Search.Models.SearchOptions.SearchTypes BllSearchTypeToDalSearchType(
        SearchOptions.SearchTypes type
    )
    {
        switch (type)
        {
            case SearchOptions.SearchTypes.Album:
                return DAL.Search.Models.SearchOptions.SearchTypes.Album;
            case SearchOptions.SearchTypes.Artist:
                return DAL.Search.Models.SearchOptions.SearchTypes.Artist;
            case SearchOptions.SearchTypes.Track:
                return DAL.Search.Models.SearchOptions.SearchTypes.Track;
        }

        throw new InvalidOperationException("could not convert this bll search type to a dal search type");
    }
}