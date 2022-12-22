namespace Spotify.Shared.DAL.Search.Models;

public record Search(string Query, int? Offset = null, int? Limit = null)
{
    public string Query { get; set; } = Query;
    public int? Offset { get; set; } = Offset;
    public int? Limit { get; set; } = Limit;
}