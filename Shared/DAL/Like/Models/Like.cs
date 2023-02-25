using Spotify.Shared.DAL.Like.Models;

namespace Spotify.Shared.DAL.Like.Models;

public record Like(
    string Id,
    string AssociatedId,
    string AssociatedUser,
    AssociatedType AssociatedType,
    DateTime CreatedAt
)
{
}