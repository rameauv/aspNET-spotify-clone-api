using AutoMapper;
using Spotify.Shared.BLL.Search.Models;
using SpotifyApi.Models;

namespace SpotifyApi.AutoMapper;

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