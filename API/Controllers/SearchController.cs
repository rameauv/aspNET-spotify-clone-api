using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using Api.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spotify.Shared.BLL.Search;
using Spotify.Shared.BLL.Search.Models;

namespace Api.Controllers;

[Route("[controller]")]
[Authorize]
[ApiController]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json, "application/problem+json")]
[ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorsDto))]
[ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorsDto))]
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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SearchResultDto))]
    public async Task<ActionResult<BaseSearchResultDto>> Search([Required] string q, int? offset, int? limit)
    {
        var res = await _searchService.SearchAsync(new Search(q)
        {
            Limit = limit,
            Offset = offset
        });
        var searchResultDto = _mapper.Map<SearchResultDto>(res);
        return Ok(searchResultDto);
    }
}