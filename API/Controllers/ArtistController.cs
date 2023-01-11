using System.Net;
using System.Net.Mime;
using Api.ExceptionFilters;
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
public class ArtistController : ControllerBase
{
    private readonly IArtistService _artistService;

    public ArtistController(IArtistService artistService)
    {
        _artistService = artistService;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ArtistDto))]
    public async Task<ActionResult<BaseSearchResultDto>> Search(string id)
    {
        var res = await _artistService.GetAsync(id);
        return Ok(new ArtistDto(
            res.Id,
            res.Name,
            res.ThumbnailUrl,
            res.LikeId,
            res.MonthlyListeners
        ));
    }

    [HttpPatch("Like")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LikeDto))]
    public async Task<ActionResult> SetLike(SetLikeRequest setLikeRequest)
    {
        // Remove "Bearer "
        var accessToken = Request.Headers.Authorization.ToString()[7..];
        if (accessToken == null)
        {
            throw new Exception("no access token provided");
        }

        var like = await _artistService.SetLikeAsync(setLikeRequest.AssociatedId, accessToken);

        return Ok(new LikeDto(like.Id));
    }
}