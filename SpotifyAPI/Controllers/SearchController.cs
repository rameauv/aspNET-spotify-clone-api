using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Spotify.Shared.BLL.Search;
using Spotify.Shared.BLL.Search.Models;
using SpotifyApi.Models;

namespace SpotifyApi.Controllers;

[Route("[controller]")]
[ApiController]
public class SearchController : ControllerBase
{
    private readonly ISearchService _searchService;
    private readonly IMapper _mapper;

    public SearchController(ISearchService searchService, IMapper mapper)
    {
        this._searchService = searchService;
        this._mapper = mapper;
    }

    [HttpGet("Search")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultDto<SearchResultDto>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResultDto<SearchResultDto>))]
    public async Task<ActionResult<ResultDto<SearchResultDto>>> Search(string q, int? offset, int? limit)
    {
        try
        {
            var res = await _searchService.SearchAsync(new Search(q)
            {
                Limit = limit,
                Offset = offset
            });
            var searchResultDto = _mapper.Map<SearchResultDto>(res);
            var result = new ResultDto<SearchResultDto>
            {
                Result = searchResultDto
            };
            return Ok(result);
        }
        catch (Exception e)
        {
            await Console.Error.WriteLineAsync(e.Message);
            await Console.Error.WriteLineAsync(e.StackTrace);
            var error = new ErrorDto(HttpStatusCode.InternalServerError, "internal server error");
            var result = new ResultDto<SearchResultDto>
            {
                Error = error
            };
            return StatusCode(500, result);
        }
    }
}