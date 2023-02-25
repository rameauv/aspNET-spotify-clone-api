using System.ComponentModel.DataAnnotations;

namespace Spotify.Shared.BLL.Shared.Items;

public class SimpleTrack: SimpleItemBase
{
    public SimpleTrack(string id, string? thumbnailUrl, string title, string artistName) : base(id, thumbnailUrl)
    {
        Title = title;
        ArtistName = artistName;
    }

    [Required] public string Title { get; set; }
    [Required] public string ArtistName { get; set; }
}