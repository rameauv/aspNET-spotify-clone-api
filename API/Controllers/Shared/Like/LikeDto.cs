using System.ComponentModel.DataAnnotations;

namespace Api.Controllers.Shared.Like;

public class LikeDto
{
    public LikeDto(string id)
    {
        Id = id;
    }

    [Required]
    public string Id { get; }
}