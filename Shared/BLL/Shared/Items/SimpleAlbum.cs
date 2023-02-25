using System.ComponentModel.DataAnnotations;

namespace Spotify.Shared.BLL.Shared.Items;

public class SimpleAlbum : SimpleItemBase
{
    [Required] public string Title { get; set; }
    [Required] public string ArtistName { get; set; }

    public SimpleAlbum(string id, string? thumbnailUrl, string title, string artistName) : base(id,
        thumbnailUrl)
    {
        Title = title;
        ArtistName = artistName;
    }
}