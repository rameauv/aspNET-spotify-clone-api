using System.ComponentModel.DataAnnotations;

namespace Api.Models.Items;

public class SimpleAlbumDto : SimpleItemBaseDto
{
    [Required] public string Title { get; set; }
    [Required] public string ArtistName { get; set; }
    [Required] public string AlbumType { get; set; }

    public SimpleAlbumDto(string id, string? thumbnailUrl, string title, string artistName, string albumType) : base(id,
        thumbnailUrl)
    {
        Title = title;
        ArtistName = artistName;
        AlbumType = albumType;
    }
}