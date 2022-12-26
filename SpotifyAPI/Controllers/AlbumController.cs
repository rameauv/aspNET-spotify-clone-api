using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spotify.Shared.BLL.Album;
using Spotify.Shared.BLL.Album.Models;
using SpotifyApi.Models;

namespace SpotifyApi.Controllers;

[Route("[controller]")]
[ApiController]
public class AlbumController : ControllerBase
{
    private readonly IAlbumService _albumService;

    public AlbumController(IAlbumService albumService)
    {
        this._albumService = albumService;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BaseResultDto<AlbumDto>))]
    public async Task<ActionResult<BaseResultDto<AlbumDto>>> Get(string id)
    {
        try
        {
            var res = await _albumService.GetAsync(id);
            var result = new BaseResultDto<AlbumDto>
            {
                Result = new AlbumDto(
                    res.Id,
                    res.Title,
                    res.ReleaseDate,
                    res.ThumbnailUrl,
                    res.ArtistId,
                    res.ArtistName,
                    res.ArtistThumbnailUrl,
                    res.AlbumType,
                    res.LikeId
                )
            };
            return Ok(result);
        }
        catch (Exception e)
        {
            await Console.Error.WriteLineAsync(e.Message);
            await Console.Error.WriteLineAsync(e.StackTrace);
            var error = new ErrorDto(HttpStatusCode.InternalServerError, "internal server error");
            var result = new BaseResultDto<AlbumDto>
            {
                Error = error
            };
            return StatusCode(500, result);
        }
    }

    [HttpGet("{id}/Tracks")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BaseResultDto<AlbumTracksDto>))]
    public async Task<ActionResult<BaseResultDto<AlbumTracksDto>>> Tracks(string id, int? limit, int? offset)
    {
        try
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
            var result = new BaseResultDto<AlbumTracksDto>
            {
                Result = new AlbumTracksDto(
                    mappedTracks,
                    res.Limit,
                    res.Offset,
                    res.Total
                )
            };
            return Ok(result);
        }
        catch (Exception e)
        {
            await Console.Error.WriteLineAsync(e.Message);
            await Console.Error.WriteLineAsync(e.StackTrace);
            var error = new ErrorDto(HttpStatusCode.InternalServerError, "internal server error");
            var result = new BaseResultDto<AlbumDto>
            {
                Error = error
            };
            return StatusCode(500, result);
        }
    }

    [HttpPatch("Like")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResultDto<LikeDto>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResultDto<LikeDto>))]

    public async Task<ActionResult> SetLike(SetLikeRequest setLikeRequest)
    {
        try
        {
            // Remove "Bearer "
            var accessToken = Request.Headers.Authorization.ToString()[7..];
            if (accessToken == null)
            {
                throw new Exception("no access token provided");
            }

            var like = await _albumService.SetLikeAsync(setLikeRequest.AssociatedId, accessToken);
            var result = new BaseResultDto<LikeDto>
            {
                Result = new LikeDto(like.Id)
            };
            return Ok(result);
        }
        catch (Exception e)
        {
            await Console.Error.WriteLineAsync(e.Message);
            await Console.Error.WriteLineAsync(e.StackTrace);
            var error = new ErrorDto(HttpStatusCode.InternalServerError, "internal server error");
            var result = new BaseResultDto<LikeDto>
            {
                Error = error
            };
            return StatusCode(500, result);
        }
    }
}