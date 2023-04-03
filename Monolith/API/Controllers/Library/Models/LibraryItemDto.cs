using System.ComponentModel.DataAnnotations;
using Api.Controllers.Shared.Items.Models;

namespace Api.Controllers.Library.Models;

public class LibraryItemDto<T> where T : SimpleItemBaseDto
{
    public LibraryItemDto(T item, DateTime likeCreatedAt, string? likeId)
    {
        LikeCreatedAt = likeCreatedAt;
        Item = item;
        LikeId = likeId;
    }

    [Required] public T Item { get; set; }
    [Required] public DateTime LikeCreatedAt { get; set; }
    [Required] public string? LikeId { get; set; }
}