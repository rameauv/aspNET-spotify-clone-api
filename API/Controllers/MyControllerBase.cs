using System.Security.Claims;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Spotify.Shared.BLL.Jwt;

namespace Api.Controllers;

public class MyControllerBase : ControllerBase
{
    private readonly IJwtService _jwtService;

    public MyControllerBase(IJwtService jwtService)
    {
        _jwtService = jwtService;
    }

    protected IActionResult Error(ErrorDto error)
    {
        Response.ContentType = "application/problem+json";
        return new ErrorResult(error);
    }

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

    protected string GetCurrentUserId()
    {
        var accessToken = GetAccessToken();
        if (accessToken == null)
        {
            throw new Exception("no access token provided");
        }

        var validatedToken = _jwtService.GetValidatedAccessToken(accessToken);
        var userId = validatedToken.Principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            throw new ArgumentException("no userid in this access token");
        }

        return userId;
    }
}