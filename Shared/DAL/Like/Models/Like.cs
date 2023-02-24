using Spotify.Shared.DAL.Like.Models;

namespace Spotify.Shared.DAL.Like.Models;

public record Like(string Id, string AssociatedId, string AssociatedUser, AssociatedType AssociatedType)
{
    public string Id { get; set; } = Id;

    public string AssociatedId { get; set; } = AssociatedId;

    public string AssociatedUser { get; set; } = AssociatedUser;

    public AssociatedType AssociatedType { get; set; } = AssociatedType;
}