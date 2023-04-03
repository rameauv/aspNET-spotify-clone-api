using Repositories.Repositories.Like.Models;
using Spotify.Shared.DAL.Like.Models;

namespace Repositories.Repositories.Like.Extensions;

public static class DbLikeExtensions
{
    public static Spotify.Shared.DAL.Like.Models.Like ToDalLike(this Contexts.Like like)
    {
        return new Spotify.Shared.DAL.Like.Models.Like(
            like.Id.ToString(),
            like.AssociatedId,
            like.AssociatedUser,
            MapDbAssociatedTypesToDalAssociatedTypes(like.AssociatedType),
            like.CreatedAt
        );
    }

    private static AssociatedType MapDbAssociatedTypesToDalAssociatedTypes(string type)
    {
        switch (type)
        {
            case var value when value == AssociatedTypes.Artist:
                return AssociatedType.Artist;
            case var value when value == AssociatedTypes.Album:
                return AssociatedType.Album;
            case var value when value == AssociatedTypes.Track:
                return AssociatedType.Track;
        }

        throw new InvalidOperationException("could not map this db associated type to a dal associated type");
    }
}