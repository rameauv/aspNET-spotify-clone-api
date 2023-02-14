namespace Api.Models;

public class SetLikeRequest
{
    public SetLikeRequest(string associatedId)
    {
        AssociatedId = associatedId;
    }

    public string AssociatedId { get; }
}