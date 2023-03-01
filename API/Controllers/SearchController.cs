using System.Net.Mime;
using Api.Controllers.Search.Models;
using Api.Controllers.Shared.Error;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spotify.Shared.BLL.Search;
using Spotify.Shared.BLL.Search.Models;

namespace Api.Controllers;

/// <summary>
/// Controller for handling search-related requests
/// </summary>
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

    /// <summary>
    /// Initializes a new instance of the <see cref="SearchController"/> class.
    /// </summary>
    /// <param name="searchService">Search service object</param>
    /// <param name="mapper">Mapper service object</param>
    public SearchController(ISearchService searchService, IMapper mapper)
    {
        this._searchService = searchService;
        this._mapper = mapper;
    }

    [HttpGet("Search")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SearchResultDto))]
    public async Task<IActionResult> Search([FromQuery] SearchQueryOptionalParams queryOptionalParams)
    {
        var searchOption = MapSearchQueryParams(queryOptionalParams);
        var res = await _searchService.SearchAsync(searchOption);
        var searchResultDto = _mapper.Map<SearchResultDto>(res);
        return Ok(searchResultDto);
    }

    private SearchOptions MapSearchQueryParams(SearchQueryOptionalParams queryOptionalParams)
    {
        var convertedType = queryOptionalParams.Types?.Split(",").Aggregate<string, SearchOptions.SearchTypes>(0, (acc, type) =>
        {
            switch (type)
            {
                case "artist":
                    return acc | SearchOptions.SearchTypes.Artist;
                case "album":
                    return acc | SearchOptions.SearchTypes.Album;
                case "track":
                    return acc | SearchOptions.SearchTypes.Track;
            }

            return acc;
        });
        var type = convertedType == 0 ? null : convertedType;

        return new SearchOptions(queryOptionalParams.Q)
        {
            Types = type,
            Limit = queryOptionalParams.Limit,
            Offset = queryOptionalParams.Offset
        };
    }
}