using Spotify.Shared.DAL.Shared;

namespace Spotify.Shared.DAL.Like.Models;

public class FindLikesByUserIdResult : Pagging<Like>
{
    public FindLikesByUserIdResult(
        IEnumerable<Like> items,
        int limit,
        int offset,
        int total
    ) : base(items, limit, offset, total)
    {
    }
}