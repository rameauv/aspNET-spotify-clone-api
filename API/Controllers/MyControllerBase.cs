using Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class MyControllerBase : ControllerBase
{
    public IActionResult Error(ErrorDto error)
    {
        Response.ContentType = "application/problem+json";
        return new ErrorResult(error);
    }
}