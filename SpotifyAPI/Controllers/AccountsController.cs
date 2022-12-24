using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spotify.Shared.BLL.MyIdentity;
using Spotify.Shared.BLL.MyIdentity.Models;
using SpotifyApi.Models;

namespace SpotifyApi.Controllers;

[Route("[controller]")]
[ApiController]
public class AccountsController : ControllerBase
{
    private readonly IMyIdentityService _identityService;
    private readonly ILogger<AccountsController> _logger;

    public AccountsController(IMyIdentityService identityService, ILogger<AccountsController> logger)
    {
        this._identityService = identityService;
        this._logger = logger;
    }

    [HttpPost("Register")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Register(CreateUserDto userModel)
    {
        Console.WriteLine(userModel);
        var res = await this._identityService.Register(new RegisterUser(
            userModel.username,
            userModel.password,
            userModel.data
        ));
        if (!res.Succeeded)
        {
            var error = res.Errors
                .Select(error => error.ToString() ?? "")
                .Aggregate((prec, current) => $"{prec}\n{current}");
            await Console.Error.WriteLineAsync(error);
            _logger.LogError("error at {DT}", 
                DateTime.UtcNow.ToLongTimeString());
            _logger.LogError( JsonSerializer.Serialize(res.Errors.ToArray()));
            return Problem(error);
        }

        return StatusCode(201);
    }

    [Authorize]
    [HttpPost("RefreshAccessToken")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(NewAccessTokenDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<NewAccessTokenDto>> RefreshAccessToken()
    {
        var refreshToken = Request.Cookies["X-Refresh-Token"];
        if (refreshToken == null)
        {
            return Unauthorized("Invalid Authentication");
        }

        var newToken = await _identityService.RefreshAccessToken(refreshToken);
        if (newToken == null)
        {
            return Unauthorized("Invalid Authentication");
        }

        return new NewAccessTokenDto(newToken.AccessToken);
    }

    [HttpPost("Login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<NewAccessTokenDto>> Login(LoginCredentialsDto credentialsModel)
    {
        var token = await this._identityService.Login(new LoginCredentials(
            credentialsModel.Username,
            credentialsModel.Password,
            credentialsModel.DeviceId
        ));
        if (token != null)
        {
            Response.Cookies.Append(
                "X-Refresh-Token",
                token.RefreshToken,
                new CookieOptions()
                {
                    HttpOnly = true,
                    SameSite = SameSiteMode.None,
                    Secure = true,
                });
            return Ok(token.AccessToken);
        }

        return Unauthorized("Invalid Authentication");
    }
}