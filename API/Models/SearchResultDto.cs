using System.ComponentModel.DataAnnotations;

namespace Api.Models;

public record SearchResultDto(ReleaseSearchResultDto[] AlbumResult, SongSearchResultDto[] SongResult, ArtistSearchResultDto[] ArtistResult)
{
    [Required]
    public ReleaseSearchResultDto[] AlbumResult { get; set; } = AlbumResult;
    [Required]
    public SongSearchResultDto[] SongResult { get; set; } = SongResult;
    [Required]
    public ArtistSearchResultDto[] ArtistResult { get; set; } = ArtistResult;
}

public class BaseSearchResultDto
{
    public BaseSearchResultDto(string id, string? thumbnailUrl)
    {
        Id = id;
        ThumbnailUrl = thumbnailUrl;
    }

    [Required]
    public string Id { get; set; }
    [Required]
    public string? ThumbnailUrl { get; set; }
}

public class ReleaseSearchResultDto : BaseSearchResultDto
{
    [Required]
    public string Title { get; set; }
    [Required]
    public string ArtistName { get; set; }

    public ReleaseSearchResultDto(string id, string? thumbnailUrl, string title, string artistName) : base(id, thumbnailUrl)
    {
        Title = title;
        ArtistName = artistName;
    }
}

public class SongSearchResultDto : BaseSearchResultDto
{
    [Required]
    public string Title { get; set; }
    [Required]
    public string ArtistName { get; set; }

    public SongSearchResultDto(string id, string? thumbnailUrl, string title, string artistName) : base(id, thumbnailUrl)
    {
        Title = title;
        ArtistName = artistName;
    }
}

public class ArtistSearchResultDto : BaseSearchResultDto
{
    [Required]
    public string Name { get; set; }

    public ArtistSearchResultDto(string id, string? thumbnailUrl, string name) : base(id, thumbnailUrl)
    {
        Name = name;
    }
}