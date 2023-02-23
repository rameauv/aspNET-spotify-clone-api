namespace Repositories.Repositories.Like.Models;

public class Like: Contexts.Like
{
    public Like(Guid id, string associatedId, string associatedUser, string associatedType)
    {
        base.Id = id;
        AssociatedId = associatedId;
        AssociatedUser = associatedUser;
        AssociatedType = associatedType;
    }
}
