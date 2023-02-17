namespace Spotify.Shared.BLL.Search.Models;

public record SearchResult(ReleaseResult[] AlbumResult, SongResult[] SongResult, ArtistResult[] ArtistResult)
{
    public ReleaseResult[] AlbumResult { get; set; } = AlbumResult;
    public SongResult[] SongResult { get; set; } = SongResult;
    public ArtistResult[] ArtistResult { get; set; } = ArtistResult;
}

public class BaseResult
{
    public BaseResult(string id, string? thumbnailUrl)
    {
        Id = id;
        ThumbnailUrl = thumbnailUrl;
    }

    public string Id { get; set; }
    public string? ThumbnailUrl { get; set; }
}

public class ReleaseResult : BaseResult
{
    public string Title { get; set; }
    public string ArtistName { get; set; }

    public ReleaseResult(string id, string? thumbnailUrl, string title, string artistName) : base(id, thumbnailUrl)
    {
        Title = title;
        ArtistName = artistName;
    }
}

public class SongResult : BaseResult
{
    public string Title { get; set; }
    public string ArtistName { get; set; }

    public SongResult(string id, string? thumbnailUrl, string title, string artistName) : base(id, thumbnailUrl)
    {
        Title = title;
        ArtistName = artistName;
    }
}

public class ArtistResult : BaseResult
{
    public string Name { get; set; }

    public ArtistResult(string id, string? thumbnailUrl, string name) : base(id, thumbnailUrl)
    {
        Name = name;
    }
}