using System.Net.Mime;
using Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spotify.Shared.BLL.Artist;

namespace Api.Controllers;

[Route("[controller]")]
[Authorize]
[ApiController]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json, "application/problem+json")]
[ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorsDto))]
[ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorsDto))]
public class ArtistController : MyControllerBase
{
    private readonly IArtistService _artistService;

    public ArtistController(IArtistService artistService)
    {
        _artistService = artistService;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ArtistDto))]
    public async Task<IActionResult> Search(string id)
    {
        var res = await _artistService.GetAsync(id);
        if (res == null)
        {
            return Error(new ErrorDto(
                "bad request",
                StatusCodes.Status400BadRequest,
                "invalid id"
            ));
        }

        return Ok(new ArtistDto(
            res.Id,
            res.Name,
            res.ThumbnailUrl,
            res.LikeId,
            res.MonthlyListeners
        ));
    }

    [HttpPatch("{id}/Like")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LikeDto))]
    public async Task<ActionResult> SetLike(string id)
    {
        var accessToken = GetAccessToken();
        if (accessToken == null)
        {
            throw new Exception("no access token provided");
        }

        var like = await _artistService.SetLikeAsync(id, accessToken);

        return Ok(new LikeDto(like.Id));
    }
}