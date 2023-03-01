using System.Security.Claims;
using Api.Controllers.Shared.Error;
using Microsoft.AspNetCore.Mvc;
using Spotify.Shared.BLL.Jwt;

namespace Api.Controllers.Shared;

/// <summary>
/// Base class for controller that contain usefully methods
/// </summary>
public class MyControllerBase : ControllerBase
{
    private readonly IJwtService _jwtService;

    public MyControllerBase(IJwtService jwtService)
    {
        _jwtService = jwtService;
    }

    /// <summary>
    /// Generate a new error response
    /// </summary>
    /// <param name="error"></param>
    /// <returns></returns>
    protected IActionResult Error(ErrorDto error)
    {
        Response.ContentType = "application/problem+json";
        return new ErrorResult(error);
    }

    /// <summary>
    /// get the current access token
    /// </summary>
    /// <returns>return the access token or null if there is currently no access token</returns>
    protected string? GetAccessToken()
    {
        // Remove "Bearer "
        var authorization = Request.Headers.Authorization.ToString();
        if (authorization.Length < 8)
        {
            return null;
        }

        return Request.Headers.Authorization.ToString()[7..];
    }

    /// <summary>
    /// get the current user's id
    /// </summary>
    /// <returns>return the current user id</returns>
    /// <exception cref="InvalidOperationException">throw an new instance of <see cref="InvalidOperationException"/> if there is currently no access token or if the token does not contain the current user's id</exception>
    protected string GetCurrentUserId()
    {
        var accessToken = GetAccessToken();
        if (accessToken == null)
        {
            throw new InvalidOperationException("no access token provided");
        }

        var validatedToken = _jwtService.GetValidatedAccessToken(accessToken);
        var userId = validatedToken.Principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            throw new InvalidOperationException("no userid in this access token");
        }

        return userId;
    }
}