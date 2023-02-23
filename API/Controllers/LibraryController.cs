using System.Net.Mime;
using Api.Models;
using Api.Models.Library;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spotify.Shared.BLL.Jwt;

namespace Api.Controllers;

/// <summary>
/// Controller for handling user's library related requests
/// </summary>
[Route("[controller]")]
[Authorize]
[ApiController]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json, "application/problem+json")]
[ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorsDto))]
[ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorsDto))]
public class LibraryController : MyControllerBase
{
    public LibraryController(IJwtService jwtService) : base(jwtService)
    {
    }

    /// <summary>
    /// Get the current user's library
    /// </summary>
    /// <returns></returns>
    [HttpGet("Get")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LibraryDto))]
    public async Task<IActionResult> Get()
    {
        var libraryDto = new LibraryDto();
        return Ok(libraryDto);
    }

    /// <summary>
    /// Find the current user's library's items
    /// </summary>
    /// <returns></returns>
    [HttpGet("Find")]
    public async Task<IActionResult> FindLibraryItems([FromQuery] FindLibraryItemsQueryParams queryParams)
    {
        var findLibraryItemsResultDto = new FindLibraryItemsResultDto();
        return Ok(findLibraryItemsResultDto);
    }

    [HttpGet]
    public async Task<IActionResult> FindLikeSongs()
    {
    }
}