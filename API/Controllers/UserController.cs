using System.Net.Mime;
using Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spotify.Shared.BLL.Jwt;
using Spotify.Shared.BLL.User;

namespace Api.Controllers;

/// <summary>
/// Controller for handling user-related requests
/// </summary>
[Route("[controller]")]
[Authorize]
[ApiController]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json, "application/problem+json")]
[ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorsDto))]
[ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorsDto))]
public class UserController : MyControllerBase
{
    private readonly IUserService _userService;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserController"/> class.
    /// </summary>
    /// <param name="userService">User service object</param>
    /// <param name="jwtService">Jwt service object</param>
    public UserController(IUserService userService, IJwtService jwtService): base(jwtService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Get the current user
    /// </summary>
    [HttpGet("CurrentUser")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CurrentUserDto))]
    public async Task<ActionResult> CurrentUser()
    {
        var userId = GetCurrentUserId();

        var user = await _userService.CurrentUserAsync(userId);
        return Ok(new CurrentUserDto(
            user.Id,
            user.Username,
            user.Name
        ));
    }

    /// <summary>
    /// Set the current user's profile name
    /// </summary>
    [HttpPatch("Name")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> SetName(SetNameRequestDto setNameRequestDto)
    {
        var userId = GetCurrentUserId();

        await _userService.SetName(userId, setNameRequestDto.Name);
        return Ok();
    }
}