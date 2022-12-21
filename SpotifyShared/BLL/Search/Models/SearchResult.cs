namespace Spotify.Shared.BLL.Search.Models;

public class SearchResult
{
    public SearchPagination Pagination { get; set; }
    public ReleaseResult[] ReleaseResults { get; set; }
    public SongResult[] SongResult { get; set; }
    public ArtistResult[] ArtistResult { get; set; }
}

public class SearchPagination
{
    uint Page { get; set; }
    uint Pages { get; set; }
    uint PerPage { get; set; }
    uint Items { get; set; }
}

public class BaseResult
{
    public string Id { get; set; }
    public uint Order { get; set; }
    public string ThumbnailUrl { get; set; }
}

public class ReleaseResult : BaseResult
{
    public string Title { get; set; }
    public string ArtistName { get; set; }
}

public class SongResult : BaseResult
{
    public string Title { get; set; }
    public string ArtistName { get; set; }
}

public class ArtistResult : BaseResult
{
    public string Name { get; set; }
}

