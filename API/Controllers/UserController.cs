using System.Net.Mime;
using Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spotify.Shared.BLL.User;

namespace Api.Controllers;

[Route("[controller]")]
[Authorize]
[ApiController]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json, "application/problem+json")]
[ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorsDto))]
[ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorsDto))]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("CurrentUser")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CurrentUserDto))]
    public async Task<ActionResult> CurrentUser()
    {
        // Remove "Bearer "
        var accessToken = Request.Headers.Authorization.ToString()[7..];
        if (accessToken == null)
        {
            throw new Exception("no access token provided");
        }

        var user = await _userService.CurrentUserAsync(accessToken);
        return Ok(new CurrentUserDto(
            user.Id,
            user.Username,
            user.Name
        ));
    }

    [HttpPatch("Name")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> SetName(SetNameRequestDto setNameRequestDto)
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
}