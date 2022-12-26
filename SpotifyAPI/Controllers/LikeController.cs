using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spotify.Shared.BLL.Like;
using SpotifyApi.Models;

namespace SpotifyApi.Controllers;

[Route("[controller]")]
[ApiController]
public class LikeController : ControllerBase
{
    private readonly ILikeService _likeService;

    public LikeController(ILikeService likesService)
    {
        this._likeService = likesService;
    }

    [HttpDelete("Delete")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> SetName(DeleteLikeDto deleteLikeDto)
    {
        try
        {
            // Remove "Bearer "
            var accessToken = Request.Headers.Authorization.ToString()[7..];
            if (accessToken == null)
            {
                throw new Exception("no access token provided");
            }

            await _likeService.DeleteAsync(deleteLikeDto.Id);
            return Ok();
        }
        catch (Exception e)
        {
            await Console.Error.WriteLineAsync(e.Message);
            await Console.Error.WriteLineAsync(e.StackTrace);
            return StatusCode(500);
        }
    }
}