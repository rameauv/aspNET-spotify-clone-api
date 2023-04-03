using Spotify.Shared.BLL.Shared.Items;

namespace Spotify.Shared.BLL.Library.Models;

public class LibraryItem<T> where T : SimpleItemBase
{
    public LibraryItem(T item, DateTime likeCreatedAt, string? likeId)
    {
        LikeCreatedAt = likeCreatedAt;
        Item = item;
        LikeId = likeId;
    }

    public T Item { get; set; }
    public string? LikeId { get; set; }
    public DateTime LikeCreatedAt;
}