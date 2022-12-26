using System.Net;
using Microsoft.AspNetCore.Authorization;
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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BaseResultDto<TrackDto>))]
    public async Task<ActionResult<BaseResultDto<TrackDto>>> Search(string id)
    {
        try
        {
            var track = await _trackRepository.GetAsync(id);
            var resultDto = new BaseResultDto<TrackDto>
            {
                Result = new TrackDto(
                    track.Id,
                    track.Title,
                    track.ArtistName,
                    track.ThumbnailUrl,
                    track.LikeId
                )
            };
            return Ok(resultDto);
        }
        catch (Exception e)
        {
            await Console.Error.WriteLineAsync(e.Message);
            await Console.Error.WriteLineAsync(e.StackTrace);
            var error = new ErrorDto(HttpStatusCode.InternalServerError, "internal server error");
            var result = new BaseResultDto<TrackDto>
            {
                Error = error
            };
            return StatusCode(500, result);
        }
    }
    
    [HttpPatch("Like")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResultDto<LikeDto>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResultDto<LikeDto>))]
    public async Task<ActionResult> SetLike(SetLikeRequest setLikeRequest)
    {
        try
        {
            // Remove "Bearer "
            var accessToken = Request.Headers.Authorization.ToString()[7..];
            if (accessToken == null)
            {
                throw new Exception("no access token provided");
            }

            var like = await _trackRepository.SetLikeAsync(setLikeRequest.AssociatedId, accessToken);
            var result = new BaseResultDto<LikeDto>
            {
                Result = new LikeDto(like.Id)
            };
            return Ok(result);
        }
        catch (Exception e)
        {
            await Console.Error.WriteLineAsync(e.Message);
            await Console.Error.WriteLineAsync(e.StackTrace);
            var error = new ErrorDto(HttpStatusCode.InternalServerError, "internal server error");
            var result = new BaseResultDto<LikeDto>
            {
                Error = error
            };
            return StatusCode(500, result);
        }
    }
}