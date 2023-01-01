namespace Api.Models;

public record SearchResultDto(ReleaseResultDto[] ReleaseResults, SongResultDto[] SongResult, ArtistResultDto[] ArtistResult)
{
    public ReleaseResultDto[] ReleaseResults { get; set; } = ReleaseResults;
    public SongResultDto[] SongResult { get; set; } = SongResult;
    public ArtistResultDto[] ArtistResult { get; set; } = ArtistResult;
}

public class BaseResultDto
{
    public BaseResultDto(string id, string? thumbnailUrl)
    {
        Id = id;
        ThumbnailUrl = thumbnailUrl;
    }

    public string Id { get; set; }
    public string? ThumbnailUrl { get; set; }
}

public class ReleaseResultDto : BaseResultDto
{
    public string Title { get; set; }
    public string ArtistName { get; set; }

    public ReleaseResultDto(string id, string? thumbnailUrl, string title, string artistName) : base(id, thumbnailUrl)
    {
        Title = title;
        ArtistName = artistName;
    }
}

public class SongResultDto : BaseResultDto
{
    public string Title { get; set; }
    public string ArtistName { get; set; }

    public SongResultDto(string id, string? thumbnailUrl, string title, string artistName) : base(id, thumbnailUrl)
    {
        Title = title;
        ArtistName = artistName;
    }
}

public class ArtistResultDto : BaseResultDto
{
    public string Name { get; set; }

    public ArtistResultDto(string id, string? thumbnailUrl, string name) : base(id, thumbnailUrl)
    {
        Name = name;
    }
}