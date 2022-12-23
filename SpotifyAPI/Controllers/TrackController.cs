using System.Net;
using Microsoft.AspNetCore.Mvc;
using Spotify.Shared.BLL.Track;
using SpotifyApi.Models;

namespace SpotifyApi.Controllers;

[Route("[controller]")]
[ApiController]
public class TrackController : ControllerBase
{
    private readonly ITrackService _trackRepository;

    public TrackController(ITrackService trackService)
    {
        _trackRepository = trackService;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultDto<TrackDto>))]
    public async Task<ActionResult<ResultDto<TrackDto>>> Search(string id)
    {
        try
        {
            var track = await _trackRepository.GetAsync(id);
            var resultDto = new ResultDto<TrackDto>
            {
                Result = new TrackDto(
                    track.Id,
                    track.Title,
                    track.ArtistName,
                    track.ThumbnailUrl,
                    track.IsLiked
                )
            };
            return Ok(resultDto);
        }
        catch (Exception e)
        {
            await Console.Error.WriteLineAsync(e.Message);
            await Console.Error.WriteLineAsync(e.StackTrace);
            var error = new ErrorDto(HttpStatusCode.InternalServerError, "internal server error");
            var result = new ResultDto<TrackDto>
            {
                Error = error
            };
            return StatusCode(500, result);
        }
    }
}