using System.Net.Mime;
using Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spotify.Shared.BLL.Album;
using Spotify.Shared.BLL.Album.Models;

namespace Api.Controllers;

[Route("[controller]")]
[Authorize]
[ApiController]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json, "application/problem+json")]
[ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorsDto))]
[ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorsDto))]
public class AlbumController : MyControllerBase
{
    private readonly IAlbumService _albumService;

    public AlbumController(IAlbumService albumService)
    {
        this._albumService = albumService;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AlbumDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorsDto))]
    public async Task<IActionResult> Get(string id)
    {
        var res = await _albumService.GetAsync(id);
        
        var result = new AlbumDto(
            res.Id,
            res.Title,
            res.ReleaseDate,
            res.ThumbnailUrl,
            res.ArtistId,
            res.ArtistName,
            res.ArtistThumbnailUrl,
            res.AlbumType,
            res.LikeId
        );
        return Ok(result);
    }

    [HttpGet("{id}/Tracks")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AlbumTracksDto))]
    public async Task<ActionResult<BaseSearchResultDto>> Tracks(string id, int? limit, int? offset)
    {
        var res = await _albumService.GetTracksAsync(id, new AlbumTracksRequest
        {
            Limit = limit,
            Offset = offset
        });
        var mappedTracks = res.Items.Select(track => new SimpleTrackDto(
            track.Id,
            track.Title,
            track.ArtistName
        )).ToArray();
        var result = new AlbumTracksDto(
            mappedTracks,
            res.Limit,
            res.Offset,
            res.Total
        );
        return Ok(result);
    }

    [HttpPatch("Like")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LikeDto))]
    public async Task<ActionResult> SetLike(SetLikeRequest setLikeRequest)
    {
        // Remove "Bearer "
        var accessToken = Request.Headers.Authorization.ToString()[7..];
        if (accessToken == null)
        {
            throw new Exception("no access token provided");
        }

        var like = await _albumService.SetLikeAsync(setLikeRequest.AssociatedId, accessToken);

        return Ok(new LikeDto(like.Id));
    }
}