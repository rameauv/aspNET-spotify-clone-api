namespace Repositories.Contexts;

public partial class Like
{
    public Guid Id { get; set; }

    public string AssociatedId { get; set; } = null!;

    public string AssociatedUser { get; set; } = null!;

    public string AssociatedType { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
}
