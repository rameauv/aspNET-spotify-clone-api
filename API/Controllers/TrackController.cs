using System.Net.Mime;
using Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spotify.Shared.BLL.Track;

namespace Api.Controllers;

[Route("[controller]")]
[Authorize]
[ApiController]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json, "application/problem+json")]
[ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorsDto))]
[ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorsDto))]
public class TrackController : MyControllerBase
{
    private readonly ITrackService _trackRepository;

    public TrackController(ITrackService trackService)
    {
        _trackRepository = trackService;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TrackDto))]
    public async Task<IActionResult> Search(string id)
    {
        var track = await _trackRepository.GetAsync(id);
        if (track == null)
        {
            return Error(new ErrorDto(
                "bad request",
                StatusCodes.Status400BadRequest,
                "invalid id"
            ));
        }

        return Ok(new TrackDto(
            track.Id,
            track.Title,
            track.ArtistName,
            track.ThumbnailUrl,
            track.LikeId
        ));
    }

    [HttpPatch("{id}/Like")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LikeDto))]
    public async Task<ActionResult> SetLike(string id)
    {
        // Remove "Bearer "
        var accessToken = Request.Headers.Authorization.ToString()[7..];
        if (accessToken == null)
        {
            throw new Exception("no access token provided");
        }

        var like = await _trackRepository.SetLikeAsync(id, accessToken);
        return Ok(new LikeDto(like.Id));
    }
}