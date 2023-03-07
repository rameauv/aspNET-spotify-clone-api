using System.ComponentModel.DataAnnotations;

namespace Spotify.Shared.BLL.Shared.Items;

public class SimpleArtist : SimpleItemBase
{
    [Required] public string Name { get; set; }

    public SimpleArtist(string id, string? thumbnailUrl, string name) 
        : base(id, thumbnailUrl)
    {
        Name = name;
    }
}