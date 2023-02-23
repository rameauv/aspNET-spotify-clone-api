using Spotify.Shared.DAL.Shared;

namespace Spotify.Shared.DAL.Like.models;

public record FindLikesByUserIdOptions(PaginationOptions Pagination)
{
    public IEnumerable<AssociatedType>? AssociatedTypes { get; init; }
}