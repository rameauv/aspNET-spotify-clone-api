using System.Diagnostics;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spotify.Shared.BLL.User;
using SpotifyApi.Models;

namespace SpotifyApi.Controllers;

[Route("[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [Authorize]
    [HttpGet("CurrentUser")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResultDto<CurrentUserDto>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResultDto<CurrentUserDto>))]
    public async Task<ActionResult> CurrentUser()
    {
        try
        {
            // Remove "Bearer "
            var accessToken = Request.Headers.Authorization.ToString()[7..];
            if (accessToken == null)
            {
                throw new Exception("no access token provided");
            }

            var user = await _userService.CurrentUserAsync(accessToken);
            var resultDto = new BaseResultDto<CurrentUserDto>
            {
                Result = new CurrentUserDto(
                    user.Id,
                    user.Username,
                    user.Name
                )
            };
            return Ok(resultDto);
        }
        catch (Exception e)
        {
            await Console.Error.WriteLineAsync(e.Message);
            await Console.Error.WriteLineAsync(e.StackTrace);
            var error = new ErrorDto(HttpStatusCode.InternalServerError, "internal server error");
            var result = new BaseResultDto<CurrentUserDto>
            {
                Error = error
            };
            return StatusCode(500, result);
        }
    }

    [HttpPatch("Name")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> SetName(SetNameRequestDto setNameRequestDto)
    {
        try
        {
            // Remove "Bearer "
            var accessToken = Request.Headers.Authorization.ToString()[7..];
            if (accessToken == null)
            {
                throw new Exception("no access token provided");
            }

            await _userService.SetName(accessToken, setNameRequestDto.Name);
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