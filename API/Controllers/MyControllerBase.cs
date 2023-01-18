using Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class MyControllerBase : ControllerBase
{
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
}