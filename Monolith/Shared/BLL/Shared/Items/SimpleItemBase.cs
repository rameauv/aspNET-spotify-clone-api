namespace Spotify.Shared.BLL.Shared.Items;

public class SimpleItemBase
{
    public SimpleItemBase(string id, string? thumbnailUrl)
    {
        Id = id;
        ThumbnailUrl = thumbnailUrl;
    }

    public string Id { get; set; }
    public string? ThumbnailUrl { get; set; }
}