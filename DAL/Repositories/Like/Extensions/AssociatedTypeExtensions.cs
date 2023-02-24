using Repositories.Repositories.Like.Models;
using Spotify.Shared.DAL.Like.Models;

namespace Repositories.Repositories.Like.Extensions;

public static class AssociatedTypeExtensions
{
    public static IEnumerable<string> ToStringArray(this IEnumerable<AssociatedType> associatedTypes)
    {
        return associatedTypes.Select(type =>
        {
            switch (type)
            {
                case AssociatedType.Album:
                    return AssociatedTypes.Album;
                case AssociatedType.Track:
                    return AssociatedTypes.Track;
                case AssociatedType.Artist:
                    return AssociatedTypes.Artist;
            }

            throw new InvalidOperationException($"could not map this associatedType, {type}");
        });
    }
}