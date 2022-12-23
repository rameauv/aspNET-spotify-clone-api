using System.Net;
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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultDto<AlbumDto>))]
    public async Task<ActionResult<ResultDto<AlbumDto>>> Get(string id)
    {
        try
        {
            var res = await _albumService.GetAsync(id);
            var result = new ResultDto<AlbumDto>
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
                    res.IsLiked
                )
            };
            return Ok(result);
        }
        catch (Exception e)
        {
            await Console.Error.WriteLineAsync(e.Message);
            await Console.Error.WriteLineAsync(e.StackTrace);
            var error = new ErrorDto(HttpStatusCode.InternalServerError, "internal server error");
            var result = new ResultDto<AlbumDto>
            {
                Error = error
            };
            return StatusCode(500, result);
        }
    }

    [HttpGet("{id}/Tracks")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultDto<AlbumTracksDto>))]
    public async Task<ActionResult<ResultDto<AlbumTracksDto>>> Tracks(string id, int? limit, int? offset)
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
            var result = new ResultDto<AlbumTracksDto>
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
            var result = new ResultDto<AlbumDto>
            {
                Error = error
            };
            return StatusCode(500, result);
        }
    }
}