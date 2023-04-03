namespace Spotify.Shared.BLL.Shared.Items;

public class SimpleAlbum : SimpleItemBase
{
    public string Title { get; set; }
    public string ArtistName { get; set; }
    public string AlbumType { get; set; }

    public SimpleAlbum(
        string id,
        string? thumbnailUrl,
        string title,
        string artistName,
        string albumType
    )
        : base(id, thumbnailUrl)
    {
        Title = title;
        ArtistName = artistName;
        AlbumType = albumType;
    }
}