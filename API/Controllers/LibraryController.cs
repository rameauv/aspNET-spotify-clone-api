using System.Net.Mime;
using Api.Models;
using Api.Models.Items;
using Api.Models.Library;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    [HttpGet("CurrentUserLibrary")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LibraryDto))]
    public async Task<IActionResult> Get()
    {
        var currentUserId = GetCurrentUserId();
        var res = await _libraryService.GetAsync(currentUserId);
        var libraryItemsDto = new LibraryItemsDto(
            res.Items.Albums.Select(album => new LibraryItemDto<SimpleAlbumDto>(new SimpleAlbumDto(
                    album.Item.Id,
                    album.Item.ThumbnailUrl,
                    album.Item.Title,
                    album.Item.ArtistName,
                    album.Item.AlbumType
                ),
                album.LikeCreatedAt,
                album.LikeId
            )),
            res.Items.Artists.Select(artist => new LibraryItemDto<SimpleArtistDto>(new SimpleArtistDto(
                    artist.Item.Id,
                    artist.Item.ThumbnailUrl,
                    artist.Item.Name
                ),
                artist.LikeCreatedAt,
                artist.LikeId
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
        var libraryItemsDto = new LibraryItemsDto(
            res.Albums.Select(album => new LibraryItemDto<SimpleAlbumDto>(new SimpleAlbumDto(
                    album.Item.Id,
                    album.Item.ThumbnailUrl,
                    album.Item.Title,
                    album.Item.ArtistName,
                    album.Item.AlbumType
                ),
                album.LikeCreatedAt,
                album.LikeId
            )),
            res.Artists.Select(artist => new LibraryItemDto<SimpleArtistDto>(new SimpleArtistDto(
                    artist.Item.Id,
                    artist.Item.ThumbnailUrl,
                    artist.Item.Name
                ),
                artist.LikeCreatedAt,
                artist.LikeId
            )),
            res.Total,
            res.Offset,
            res.Limit
        );
        return Ok(libraryItemsDto);
    }

    [HttpGet("FindLikedTracks")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FindLikedTracksResultDto))]
    public async Task<IActionResult> FindLikedTracks([FromQuery] FindLikedTracksQueryParams queryParams)
    {
        var currentUserId = GetCurrentUserId();
        var res = await _libraryService.FindLikedTracksAsync(
            currentUserId,
            new FindLikedTracksOptions(new PaginationOptions(queryParams.Limit, queryParams.Offset))
        );
        var itemsDto = res.Items.Select(track => new LibraryItemDto<SimpleTrack>(new SimpleTrack(
                track.Item.Id,
                track.Item.ThumbnailUrl,
                track.Item.Title,
                track.Item.ArtistName
            ),
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