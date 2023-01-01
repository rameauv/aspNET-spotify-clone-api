using AutoMapper;

namespace Api.AutoMapper;

public class BllProfile : Profile
{
    public BllProfile()
    {
        CreateMap<Spotify.Shared.DAL.Search.Models.SearchResult, Spotify.Shared.BLL.Search.Models.SearchResult>();
        CreateMap<Spotify.Shared.DAL.Search.Models.ReleaseResult, Spotify.Shared.BLL.Search.Models.ReleaseResult>();
        CreateMap<Spotify.Shared.DAL.Search.Models.ArtistResult, Spotify.Shared.BLL.Search.Models.ArtistResult>();
        CreateMap<Spotify.Shared.DAL.Search.Models.SongResult, Spotify.Shared.BLL.Search.Models.SongResult>();
        CreateMap<Spotify.Shared.DAL.Search.Models.BaseResult, Spotify.Shared.BLL.Search.Models.BaseResult>();
    }
}