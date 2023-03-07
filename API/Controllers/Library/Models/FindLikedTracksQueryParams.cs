using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Library.Models;

public class FindLikedTracksQueryParams
{
    [FromQuery(Name = "offset")] public int Offset { get; set; }
    [FromQuery(Name = "limit")] public int Limit { get; set; }
}