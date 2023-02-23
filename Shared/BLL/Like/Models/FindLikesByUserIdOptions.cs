using Spotify.Shared.BLL.Shared;

namespace Spotify.Shared.BLL.Like.Models;

public record FindLikesByUserIdOptions(PaginationOptions Pagination)
{
    public IEnumerable<AssociatedType>? AssociatedTypes { get; init; }
}