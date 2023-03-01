using AutoMapper;
using AutoMapper.Extensions.EnumMapping;

namespace Api.AutoMapper;

public class BllProfile : Profile
{
    public BllProfile()
    {
        CreateMap<Spotify.Shared.DAL.Search.Models.SearchResult, Spotify.Shared.BLL.Search.Models.SearchResult>();
        CreateMap<Spotify.Shared.DAL.Search.Models.AlbumResult, Spotify.Shared.BLL.Search.Models.ReleaseResult>();
        CreateMap<Spotify.Shared.DAL.Search.Models.ArtistResult, Spotify.Shared.BLL.Search.Models.ArtistResult>();
        CreateMap<Spotify.Shared.DAL.Search.Models.SongResult, Spotify.Shared.BLL.Search.Models.SongResult>();
        CreateMap<Spotify.Shared.DAL.Search.Models.BaseResult, Spotify.Shared.BLL.Search.Models.BaseResult>();
        
        CreateMap<Spotify.Shared.DAL.Like.Models.AssociatedType, Spotify.Shared.BLL.Like.Models.AssociatedType>()
            .ConvertUsingEnumMapping(opt => opt
                .MapByName()
            );
        CreateMap<Spotify.Shared.DAL.Track.Models.Track, Spotify.Shared.BLL.Shared.Items.SimpleTrack>();
        CreateMap<Spotify.Shared.DAL.Album.Models.Album, Spotify.Shared.BLL.Shared.Items.SimpleAlbum>();
        CreateMap<Spotify.Shared.DAL.Artist.Models.Artist, Spotify.Shared.BLL.Shared.Items.SimpleArtist>();
    }
}