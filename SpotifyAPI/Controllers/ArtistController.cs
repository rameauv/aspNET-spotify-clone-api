using System.Net;
using Microsoft.AspNetCore.Mvc;
using Spotify.Shared.BLL.Artist;
using SpotifyApi.Models;

namespace SpotifyApi.Controllers;

[Route("[controller]")]
[ApiController]
public class ArtistController : ControllerBase
{
    private readonly IArtistService _artistService;

    public ArtistController(IArtistService artistService)
    {
        _artistService = artistService;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultDto<TrackDto>))]
    public async Task<ActionResult<ResultDto<ArtistDto>>> Search(string id)
    {
        try
        {
            var res = await _artistService.GetAsync(id);
            var result = new ResultDto<ArtistDto>
            {
                Result = new ArtistDto(
                    res.Id,
                    res.Name,
                    res.ThumbnailUrl,
                    res.IsFollowing,
                    res.MonthlyListeners
                )
            };
            return Ok(result);
        }
        catch (Exception e)
        {
            await Console.Error.WriteLineAsync(e.Message);
            await Console.Error.WriteLineAsync(e.StackTrace);
            var error = new ErrorDto(HttpStatusCode.InternalServerError, "internal server error");
            var result = new ResultDto<ArtistDto>
            {
                Error = error
            };
            return StatusCode(500, result);
        }
    }
}