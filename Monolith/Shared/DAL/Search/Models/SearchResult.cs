namespace Spotify.Shared.DAL.Search.Models;

public record SearchResult(
    IEnumerable<AlbumResult> AlbumResult, 
    IEnumerable<SongResult> SongResult,
    IEnumerable<ArtistResult> ArtistResult
    )
{
    public IEnumerable<AlbumResult> AlbumResult { get; set; } = AlbumResult;
    public IEnumerable<SongResult> SongResult { get; set; } = SongResult;
    public IEnumerable<ArtistResult> ArtistResult { get; set; } = ArtistResult;
}

public class BaseResult
{
    public BaseResult(string id, string? thumbnailUrl, int popularity)
    {
        Id = id;
        ThumbnailUrl = thumbnailUrl;
        Popularity = popularity;
    }

    public string Id { get; set; }
    public string? ThumbnailUrl { get; set; }
    public int Popularity { get; set; }
    public int Order { get; set; } = 0;
}

public class AlbumResult : BaseResult
{
    public string Title { get; set; }
    public string ArtistName { get; set; }

    public AlbumResult(string id, string? thumbnailUrl, string title, string artistName, int popularity) : base(id,
        thumbnailUrl, popularity)
    {
        Title = title;
        ArtistName = artistName;
    }

    public AlbumResult(
        AlbumResult previousAlbumResult
    ) : base(previousAlbumResult.Id, previousAlbumResult.ThumbnailUrl,
        previousAlbumResult.Popularity)
    {
        Title = previousAlbumResult.Title;
        ArtistName = previousAlbumResult.ArtistName;
    }
}

public class SongResult : BaseResult
{
    public string Title { get; set; }
    public string ArtistName { get; set; }

    public SongResult(string id, string? thumbnailUrl, string title, string artistName, int popularity) : base(id,
        thumbnailUrl, popularity)
    {
        Title = title;
        ArtistName = artistName;
    }

    public SongResult(SongResult previousSongResult) : base(previousSongResult.Id, previousSongResult.ThumbnailUrl,
        previousSongResult.Popularity)
    {
        Title = previousSongResult.Title;
        ArtistName = previousSongResult.ArtistName;
    }
}

public class ArtistResult : BaseResult
{
    public string Name { get; set; }

    public ArtistResult(string id, string? thumbnailUrl, string name, int popularity) : base(id, thumbnailUrl,
        popularity)
    {
        Name = name;
    }

    public ArtistResult(
        ArtistResult previousArtistResult
    ) : base(previousArtistResult.Id,
        previousArtistResult.ThumbnailUrl, previousArtistResult.Popularity)
    {
        Name = previousArtistResult.Name;
    }
}