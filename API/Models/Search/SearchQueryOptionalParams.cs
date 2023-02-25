using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Api.Models.Search;

public class SearchQueryOptionalParams
{
    [Required] [FromQuery(Name = "q")] public string Q { get; set; } = null!;
    [FromQuery(Name = "offset")] public int? Offset { get; set; }
    [FromQuery(Name = "limit")] public int? Limit { get; set; }

    /// <summary>
    /// A comma-separated list of item types to search across. Search results include hits from all the specified item types. For example: "album,track" returns both albums and tracks".
    /// Valid values: artist, album, track
    /// If omitted search for every categories
    /// </summary>
    [FromQuery(Name = "types")]
    public string? Types { get; set; }
}