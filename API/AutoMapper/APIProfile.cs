using Api.Models;
using AutoMapper;
using Spotify.Shared.BLL.Search.Models;

namespace Api.AutoMapper;

public class ApiProfile : Profile
{
    public ApiProfile()
    {
        CreateMap<SearchResult, SearchResultDto>();
        CreateMap<ReleaseResult, ReleaseResultDto>();
        CreateMap<ArtistResult, ArtistResultDto>();
        CreateMap<SongResult, SongResultDto>();
        CreateMap<BaseResult, BaseResultDto>();
    }
}