namespace Spotify.Shared.BLL.Search.Models;

public record Search(string Query, uint? Page = null, uint? PerPage = null)
{
    public string Query { get; set; } = Query;
    public uint? Page { get; set; } = Page;
    public uint? PerPage { get; set; } = PerPage;
}