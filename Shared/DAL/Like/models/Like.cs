namespace Spotify.Shared.DAL.Like.models;

public record Like(string Id, string AssociatedId, string AssociatedUser, string? AssociatedType)
{
    public string Id { get; set; } = Id;

    public string AssociatedId { get; set; } = AssociatedId;

    public string AssociatedUser { get; set; } = AssociatedUser;

    public string? AssociatedType { get; set; } = AssociatedType;
}