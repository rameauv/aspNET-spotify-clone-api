using System.ComponentModel.DataAnnotations;

namespace SpotifyApi.Models;

public class SetLikeRequest
{
    public SetLikeRequest(string associatedId)
    {
        AssociatedId = associatedId;
    }

    [Required]
    public string AssociatedId { get; set; }
}