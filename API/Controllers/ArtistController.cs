using System.Net;
using Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spotify.Shared.BLL.Artist;

namespace Api.Controllers;

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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BaseResultDto<ArtistDto>))]
    public async Task<ActionResult<BaseResultDto<ArtistDto>>> Search(string id)
    {
        try
        {
            var res = await _artistService.GetAsync(id);
            var result = new BaseResultDto<ArtistDto>
            {
                Result = new ArtistDto(
                    res.Id,
                    res.Name,
                    res.ThumbnailUrl,
                    res.LikeId,
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
            var result = new BaseResultDto<ArtistDto>
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

            var like = await _artistService.SetLikeAsync(setLikeRequest.AssociatedId, accessToken);
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