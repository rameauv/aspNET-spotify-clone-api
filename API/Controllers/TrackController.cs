using System.Net.Mime;
using Api.Controllers.Shared;
using Api.Controllers.Shared.Error;
using Api.Controllers.Shared.Like;
using Api.Controllers.Track.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spotify.Shared.BLL.Jwt;
using Spotify.Shared.BLL.Track;

namespace Api.Controllers;

/// <summary>
/// Controller for handling track-related requests
/// </summary>
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

    /// <summary>
    /// Initializes a new instance of the <see cref="TrackController"/> class.
    /// </summary>
    /// <param name="trackService">Track service object</param>
    /// <param name="jwtService">The jwt service</param>
    public TrackController(ITrackService trackService, IJwtService jwtService): base(jwtService)
    {
        _trackRepository = trackService;
    }

    /// <summary>
    /// Get the track by its id
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TrackDto))]
    public async Task<IActionResult> Get(string id)
    {
        var userId = GetCurrentUserId();
        var track = await _trackRepository.GetAsync(id, userId);
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

    /// <summary>
    /// Set a like for the track
    /// </summary>
    [HttpPatch("{id}/Like")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LikeDto))]
    public async Task<ActionResult> SetLike(string id)
    {
        var userId = GetCurrentUserId();

        var like = await _trackRepository.SetLikeAsync(id, userId);
        return Ok(new LikeDto(like.Id));
    }
}