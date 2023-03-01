using System.Net.Mime;
using Api.Controllers.Library.Extensions;
using Api.Controllers.Library.Models;
using Api.Controllers.Shared;
using Api.Controllers.Shared.Error;
using Api.Controllers.Shared.Items.Extensions;
using Api.Controllers.Shared.Items.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spotify.Shared.BLL.Jwt;
using Spotify.Shared.BLL.Library;
using Spotify.Shared.BLL.Library.Models;
using Spotify.Shared.BLL.Like.Models;
using Spotify.Shared.BLL.Shared;

namespace Api.Controllers.Library;

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
    private readonly ILibraryService _libraryService;

    public LibraryController(IJwtService jwtService, ILibraryService libraryService) : base(jwtService)
    {
        _libraryService = libraryService;
    }

    /// <summary>
    /// Get the current user's library
    /// </summary>
    /// <returns></returns>
    [HttpGet("CurrentUserLibrary")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LibraryDto))]
    public async Task<IActionResult> Get()
    {
        var currentUserId = GetCurrentUserId();
        var res = await _libraryService.GetAsync(currentUserId);
        var libraryItemsDto = res.Items.ToDto();
        var libraryDto = new LibraryDto(
            res.LikedTracksCount,
            libraryItemsDto
        );
        return Ok(libraryDto);
    }

    /// <summary>
    /// Find the current user's library's items
    /// </summary>
    /// <returns></returns>
    [HttpGet("FindLibraryItems")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LibraryItemsDto))]
    public async Task<IActionResult> FindLibraryItems([FromQuery] FindLibraryItemsQueryParams queryParams)
    {
        var currentUserId = GetCurrentUserId();
        var paginationOptions = new PaginationOptions(queryParams.Limit, queryParams.Offset);
        var findLikesByUserIdOptions = new FindLikesByUserIdOptions(paginationOptions);
        var res = await _libraryService.FindLibraryItemsAsync(
            currentUserId,
            findLikesByUserIdOptions
        );
        var libraryItemsDto = res.ToDto();
        return Ok(libraryItemsDto);
    }

    /// <summary>
    /// Find the current user's liked tracks
    /// </summary>
    /// <param name="queryParams"></param>
    /// <returns></returns>
    [HttpGet("FindLikedTracks")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FindLikedTracksResultDto))]
    public async Task<IActionResult> FindLikedTracks([FromQuery] FindLikedTracksQueryParams queryParams)
    {
        var currentUserId = GetCurrentUserId();
        var res = await _libraryService.FindLikedTracksAsync(
            currentUserId,
            new FindLikedTracksOptions(new PaginationOptions(queryParams.Limit, queryParams.Offset))
        );
        var itemsDto = res.Items.Select(track => new LibraryItemDto<SimpleTrackDto>(
            track.Item.ToDto(),
            track.LikeCreatedAt,
            track.LikeId
        ));
        return Ok(new FindLikedTracksResultDto(
            itemsDto,
            res.Limit,
            res.Offset,
            res.Total
        ));
    }
}