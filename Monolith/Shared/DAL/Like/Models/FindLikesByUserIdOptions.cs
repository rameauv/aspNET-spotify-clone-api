using Spotify.Shared.DAL.Like.Models;
using Spotify.Shared.DAL.Shared;

namespace Spotify.Shared.DAL.Like.Models;

public record FindLikesByUserIdOptions(PaginationOptions Pagination)
{
    public IEnumerable<AssociatedType>? AssociatedTypes { get; set; }
}