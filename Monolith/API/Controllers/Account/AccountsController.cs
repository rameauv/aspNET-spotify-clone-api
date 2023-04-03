using System.Net.Mime;
using Api.Controllers.Account.Models;
using Api.Controllers.Shared;
using Api.Controllers.Shared.Error;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spotify.Shared;
using Spotify.Shared.BLL.Jwt;
using Spotify.Shared.BLL.MyIdentity;
using Spotify.Shared.BLL.MyIdentity.Models;

namespace Api.Controllers.Account;

/// <summary>
/// Controller for handling account-related requests
/// </summary>
[Route("[controller]")]
[ApiController]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json, "application/problem+json")]
[ProducesResponseType(typeof(ErrorsDto), StatusCodes.Status500InternalServerError)]
public class AccountsController : MyControllerBase
{
    private readonly IAuthService _identityService;
    private readonly ILogger<AccountsController> _logger;
    private readonly JwtConfig _jwtConfig;

    /// <summary>
    /// Initializes a new instance of the <see cref="AccountsController"/> class.
    /// </summary>
    /// <param name="identityService">Identity service object</param>
    /// <param name="logger">Logger object</param>
    /// <param name="jwtConfig">JWT Configuration object</param>
    /// <param name="jwtService">JWT service object</param>
    public AccountsController(
        IAuthService identityService,
        ILogger<AccountsController> logger,
        JwtConfig jwtConfig,
        IJwtService jwtService
    ) : base(jwtService)
    {
        this._identityService = identityService;
        this._logger = logger;
        this._jwtConfig = jwtConfig;
    }

    /// <summary>
    /// Registers a new user
    /// </summary>
    [HttpPost("Register")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Register(CreateUserDto userModel)
    {
        await this._identityService.Register(new RegisterUser(
            userModel.Username,
            userModel.Password,
            userModel.Data
        ));
        return StatusCode(201);
    }

    /// <summary>
    /// Refreshes the access token
    /// </summary>
    [HttpPost("RefreshAccessToken")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(NewAccessTokenDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorsDto))]
    public async Task<IActionResult> RefreshAccessToken()
    {
        var refreshToken = Request.Cookies["X-Refresh-Token"];
        if (refreshToken == null)
        {
            return Error(new ErrorDto(
                "Invalid Authentication",
                StatusCodes.Status401Unauthorized,
                ""));
        }

        var newToken = await _identityService.RefreshAccessToken(refreshToken);
        if (newToken == null)
        {
            _removeRefreshTokenCookie();
            return Error(new ErrorDto(
                "Invalid Authentication",
                StatusCodes.Status401Unauthorized,
                ""));
        }

        _addRefreshTokenCookie(newToken.RefreshToken);
        return Ok(new NewAccessTokenDto(newToken.AccessToken));
    }

    /// <summary>
    /// Logs in a user
    /// </summary>
    [HttpPost("Login")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(NewAccessTokenDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorsDto))]
    public async Task<IActionResult> Login(LoginCredentialsDto credentialsModel)
    {
        var token = await this._identityService.Login(new LoginCredentials(
            credentialsModel.Username,
            credentialsModel.Password
        ));
        if (token == null)
        {
            return Error(new ErrorDto(
                "Invalid Authentication",
                StatusCodes.Status401Unauthorized,
                ""));
        }

        _addRefreshTokenCookie(token.RefreshToken);

        return Ok(new NewAccessTokenDto(token.AccessToken));
    }

    /// <summary>
    /// Logs out the current user
    /// </summary>
    [HttpPost("Logout")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorsDto))]
    public async Task<IActionResult> Logout()
    {
        var refreshToken = Request.Cookies["X-Refresh-Token"];
        if (refreshToken == null)
        {
            return Error(new ErrorDto(
                "Invalid Authentication",
                StatusCodes.Status401Unauthorized,
                ""));
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