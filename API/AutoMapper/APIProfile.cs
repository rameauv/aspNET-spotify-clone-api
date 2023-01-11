using Api.Models;
using AutoMapper;
using Spotify.Shared.BLL.Search.Models;

namespace Api.AutoMapper;

public class ApiProfile : Profile
{
    public ApiProfile()
    {
        CreateMap<SearchResult, SearchResultDto>();
        CreateMap<ReleaseResult, ReleaseSearchResultDto>();
        CreateMap<ArtistResult, ArtistSearchResultDto>();
        CreateMap<SongResult, SongSearchResultDto>();
        CreateMap<BaseResult, BaseSearchResultDto>();
    }
}