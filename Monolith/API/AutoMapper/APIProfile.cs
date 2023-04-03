using Api.Controllers.Search.Models;
using Api.Controllers.Shared.Items.Models;
using AutoMapper;
using Spotify.Shared.BLL.Search.Models;
using Spotify.Shared.BLL.Shared.Items;

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
        
        CreateMap<SimpleTrack, SimpleTrackDto>();
        CreateMap<SimpleAlbum, SimpleAlbumDto>();
        CreateMap<SimpleArtist, SimpleArtistDto>();
    }
}