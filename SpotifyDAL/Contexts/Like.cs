namespace Repositories.Contexts;

public class Like
{
    public Guid Id { get; set; }

    public string AssociatedId { get; set; } = null!;

    public string AssociatedUser { get; set; } = null!;

    public string? AssociatedType { get; set; }
}
