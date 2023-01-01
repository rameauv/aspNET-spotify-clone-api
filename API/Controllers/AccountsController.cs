using System.Text.Json;
using Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spotify.Shared;
using Spotify.Shared.BLL.MyIdentity;
using Spotify.Shared.BLL.MyIdentity.Models;

namespace Api.Controllers;

[Route("[controller]")]
[ApiController]
public class AccountsController : ControllerBase
{
    private readonly IMyIdentityService _identityService;
    private readonly ILogger<AccountsController> _logger;
    private readonly JwtConfig _jwtConfig;

    public AccountsController(
        IMyIdentityService identityService,
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
        var res = await this._identityService.Register(new RegisterUser(
            userModel.Username,
            userModel.Password,
            userModel.Data
        ));
        if (!res.Succeeded)
        {
            var error = res.Errors
                .Select(error => error.ToString() ?? "")
                .Aggregate((prec, current) => $"{prec}\n{current}");
            await Console.Error.WriteLineAsync(error);
            _logger.LogError("error at {DT}",
                DateTime.UtcNow.ToLongTimeString());
            _logger.LogError(JsonSerializer.Serialize(res.Errors.ToArray()));
            return Problem(error);
        }

        return StatusCode(201);
    }

    [HttpPost("RefreshAccessToken")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(NewAccessTokenDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<NewAccessTokenDto>> RefreshAccessToken()
    {
        var cookieInThePast = new CookieOptions()
        {
            HttpOnly = true,
            SameSite = SameSiteMode.Strict,
            Secure = false,
            Expires = DateTimeOffset.Now.Subtract(TimeSpan.FromDays(365))
        };
        var refreshToken = Request.Cookies["X-Refresh-Token"];
        if (refreshToken == null)
        {
            return Unauthorized("Invalid Authentication");
        }

        var newToken = await _identityService.RefreshAccessToken(refreshToken);
        if (newToken == null)
        {
            _removeRefreshTokenCookie();
            return Unauthorized("Invalid Authentication");
        }

        Response.Cookies.Append(
            "X-Refresh-Token",
            newToken.RefreshToken,
            new CookieOptions()
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
                Secure = false,
            });
        return new NewAccessTokenDto(newToken.AccessToken);
    }

    [HttpPost("Login")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(NewAccessTokenDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<NewAccessTokenDto>> Login(LoginCredentialsDto credentialsModel)
    {
        var token = await this._identityService.Login(new LoginCredentials(
            credentialsModel.Username,
            credentialsModel.Password
        ));
        if (token == null) return Unauthorized("Invalid Authentication");
        Response.Cookies.Append(
            "X-Refresh-Token",
            token.RefreshToken,
            new CookieOptions()
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
                Secure = false,
            });
        return Ok(new NewAccessTokenDto(token.AccessToken));
    }


    [HttpPost("Logout")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<NewAccessTokenDto>> Logout()
    {
        var refreshToken = Request.Cookies["X-Refresh-Token"];
        if (refreshToken == null)
        {
            return Unauthorized("Invalid Authentication");
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
                Expires = DateTimeOffset.Now.AddMinutes(double.Parse(expiryTimeInMinutes))
            });
    }
}