using System.ComponentModel.DataAnnotations;

namespace Api.Models.Search;

public class SongSearchResultDto : BaseSearchResultDto
{
    [Required] public string Title { get; set; }
    [Required] public string ArtistName { get; set; }

    public SongSearchResultDto(string id, string? thumbnailUrl, string title, string artistName) : base(id,
        thumbnailUrl)
    {
        Title = title;
        ArtistName = artistName;
    }
}