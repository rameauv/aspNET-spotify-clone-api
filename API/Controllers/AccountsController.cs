using System.Net.Mime;
using Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spotify.Shared;
using Spotify.Shared.BLL.MyIdentity;
using Spotify.Shared.BLL.MyIdentity.Models;

namespace Api.Controllers;

[Route("[controller]")]
[ApiController]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json, "application/problem+json")]
[ProducesResponseType(typeof(ErrorsDto), StatusCodes.Status500InternalServerError)]
public class AccountsController : ControllerBase
{
    private readonly IAuthService _identityService;
    private readonly ILogger<AccountsController> _logger;
    private readonly JwtConfig _jwtConfig;

    public AccountsController(
        IAuthService identityService,
        ILogger<AccountsController> logger,
        JwtConfig jwtConfig)
    {
        this._identityService = identityService;
        this._logger = logger;
        this._jwtConfig = jwtConfig;
    }

    [HttpPost("Register")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Register(CreateUserDto userModel)
    {
        Console.WriteLine(userModel);
        await this._identityService.Register(new RegisterUser(
            userModel.Username,
            userModel.Password,
            userModel.Data
        ));
        return StatusCode(201);
    }

    [HttpPost("RefreshAccessToken")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(NewAccessTokenDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorsDto))]
    public async Task<ActionResult<NewAccessTokenDto>> RefreshAccessToken()
    {
        var refreshToken = Request.Cookies["X-Refresh-Token"];
        if (refreshToken == null)
        {
            Response.ContentType = "application/problem+json";
            return new ErrorResult(new ErrorDto(
                "Invalid Authentication",
                StatusCodes.Status401Unauthorized,
                ""
            ));
        }

        var newToken = await _identityService.RefreshAccessToken(refreshToken);
        if (newToken == null)
        {
            _removeRefreshTokenCookie();
            Response.ContentType = "application/problem+json";
            return new ErrorResult(new ErrorDto(
                "Invalid Authentication",
                StatusCodes.Status401Unauthorized,
                ""
            ));
        }

        _addRefreshTokenCookie(newToken.RefreshToken);
        return new NewAccessTokenDto(newToken.AccessToken);
    }

    [HttpPost("Login")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(NewAccessTokenDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorsDto))]
    public async Task<ActionResult<NewAccessTokenDto>> Login(LoginCredentialsDto credentialsModel)
    {
        var token = await this._identityService.Login(new LoginCredentials(
            credentialsModel.Username,
            credentialsModel.Password
        ));
        if (token == null)
        {
            Response.ContentType = "application/problem+json";
            return new ErrorResult(new ErrorDto(
                "Invalid Authentication",
                StatusCodes.Status401Unauthorized,
                ""
            ));
        }

        _addRefreshTokenCookie(token.RefreshToken);

        return Ok(new NewAccessTokenDto(token.AccessToken));
    }


    [HttpPost("Logout")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorsDto))]
    public async Task<ActionResult<NewAccessTokenDto>> Logout()
    {
        var refreshToken = Request.Cookies["X-Refresh-Token"];
        if (refreshToken == null)
        {
            Response.ContentType = "application/problem+json";
            return new ErrorResult(new ErrorDto(
                "Invalid Authentication",
                StatusCodes.Status401Unauthorized,
                ""
            ));
        }

        await _identityService.Logout(refreshToken);
        _removeRefreshTokenCookie();
        return Ok();
    }

    private void _removeRefreshTokenCookie()
    {
        Response.Cookies.Append(
            "X-Refresh-Token",
            "",
            new CookieOptions()
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
                Secure = false,
                Expires = DateTimeOffset.Now.Subtract(TimeSpan.FromDays(365))
            });
    }

    private void _addRefreshTokenCookie(string refreshToken)
    {
        var expiryTimeInMinutes = _jwtConfig.RefreshTokenExpiryInMinutes;
        Response.Cookies.Append(
            "X-Refresh-Token",
            refreshToken,
            new CookieOptions()
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
                Secure = false,
                Expires = DateTimeOffset.Now.AddMinutes(expiryTimeInMinutes)
            });
    }
}