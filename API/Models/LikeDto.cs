using System.ComponentModel.DataAnnotations;

namespace Api.Models;

public class LikeDto
{
    public LikeDto(string id)
    {
        Id = id;
    }

    [Required]
    public string Id { get; }
}