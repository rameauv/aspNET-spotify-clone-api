namespace Spotify.Shared.BLL.Search.Models;

public record SearchOptions(string Q)
{
    public int? Offset { get; init; }
    public int? Limit { get; init; }
    public SearchTypes? Types { get; init; }

    [Flags]
    public enum SearchTypes
    {
        Album = 1,
        Artist = 2,
        Track = 4,
    }
}