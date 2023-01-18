using System.Net.Mime;
using Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spotify.Shared.BLL.Like;

namespace Api.Controllers;

/// <summary>
/// Controller for handling artist-related requests
/// </summary>
[Route("[controller]")]
[Authorize]
[ApiController]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json, "application/problem+json")]
[ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorsDto))]
[ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorsDto))]
public class LikeController : MyControllerBase
{
    private readonly ILikeService _likeService;

    /// <summary>
    /// Initializes a new instance of the <see cref="LikeController"/> class.
    /// </summary>
    /// <param name="likesService">Like service object</param>
    public LikeController(ILikeService likesService)
    {
        this._likeService = likesService;
    }

    /// <summary>
    /// Delete the like by its id
    /// </summary>
    [HttpDelete("Delete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> Delete(DeleteLikeDto deleteLikeDto)
    {
        var accessToken = GetAccessToken();
        if (accessToken == null)
        {
            throw new Exception("no access token provided");
        }

        await _likeService.DeleteAsync(deleteLikeDto.Id);
        return Ok();
    }
}