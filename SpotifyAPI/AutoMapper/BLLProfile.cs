using AutoMapper;
using SharedBLL = Spotify.Shared.BLL;
using SharedDAL = Spotify.Shared.BLL;

namespace SpotifyApi.AutoMapper;

public class BllProfile : Profile
{
    public BllProfile()
    {
        CreateMap<SharedBLL.Search.Models.SearchResult, SharedDAL.Search.Models.SearchResult>();
    }
}