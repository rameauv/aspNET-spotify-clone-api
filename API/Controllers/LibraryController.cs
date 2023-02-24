using System.Net.Mime;
using Api.Models;
using Api.Models.Library;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spotify.Shared.BLL.Album.Models;
using Spotify.Shared.BLL.Artist.Models;
using Spotify.Shared.BLL.Jwt;
using Spotify.Shared.BLL.Library;
using Spotify.Shared.BLL.Library.Models;
using Spotify.Shared.BLL.Like.Models;
using Spotify.Shared.BLL.Shared;

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
    private readonly ILibraryService _libraryService;

    public LibraryController(IJwtService jwtService, ILibraryService libraryService) : base(jwtService)
    {
        _libraryService = libraryService;
    }

    /// <summary>
    /// Get the current user's library
    /// </summary>
    /// <returns></returns>
    [HttpGet("Get")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LibraryDto))]
    public async Task<IActionResult> Get()
    {
        var currentUserId = GetCurrentUserId();
        var res = await _libraryService.GetAsync(currentUserId);
        var libraryItemsDto = new LibraryItemsDto(
            res.Items.Albums.Select(album => new Album(
                album.Id,
                album.Title,
                album.ReleaseDate,
                album.ThumbnailUrl,
                album.ArtistId,
                album.ArtistName,
                album.ArtistThumbnailUrl,
                album.AlbumType,
                album.LikeId
            )),
            res.Items.Artists.Select(artist => new Artist(
                artist.Id,
                artist.Name,
                artist.ThumbnailUrl,
                artist.LikeId,
                artist.MonthlyListeners
            )),
            res.Items.Total,
            res.Items.Offset,
            res.Items.Limit
        );
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
    [HttpGet("Find")]
    public async Task<IActionResult> FindLibraryItems([FromQuery] FindLibraryItemsQueryParams queryParams)
    {
        var currentUserId = GetCurrentUserId();
        var paginationOptions = new PaginationOptions(queryParams.Limit, queryParams.Offset);
        var findLikesByUserIdOptions = new FindLikesByUserIdOptions(paginationOptions);
        var res = await _libraryService.FindLibraryItemsAsync(
            currentUserId,
            findLikesByUserIdOptions
        );
        var libraryItemsDto = new LibraryItemsDto(
            res.Albums.Select(album => new Album(
                album.Id,
                album.Title,
                album.ReleaseDate,
                album.ThumbnailUrl,
                album.ArtistId,
                album.ArtistName,
                album.ArtistThumbnailUrl,
                album.AlbumType,
                album.LikeId
            )),
            res.Artists.Select(artist => new Artist(
                artist.Id,
                artist.Name,
                artist.ThumbnailUrl,
                artist.LikeId,
                artist.MonthlyListeners
            )),
            res.Total,
            res.Offset,
            res.Limit
        );
        return Ok(libraryItemsDto);
    }

    [HttpGet]
    public async Task<IActionResult> FindLikeTracks([FromQuery] FindLikeTracksQueryParams queryParams)
    {
        var currentUserId = GetCurrentUserId();
        var res = await _libraryService.FindLikedTracksAsync(
            currentUserId,
            new FindLikedTracksOptions(new PaginationOptions(queryParams.Limit, queryParams.Offset))
        );
        var itemsDto = res.Items.Select(track => new TrackLibraryItem(
            track.Id,
            track.ThumbnailUrl,
            track.Title,
            track.ArtistName
        ));
        return Ok(new FindLikedTracksResultDto(
            itemsDto,
            res.Limit,
            res.Offset,
            res.Total
        ));
    }
}