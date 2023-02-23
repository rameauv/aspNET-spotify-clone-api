using SpotifyAPI.Web;

namespace RealSpotifyDAL.Repositories.Album.Extensions;

public static class ApiFullAlbumExtension
{
    public static Spotify.Shared.DAL.Album.Models.Album ToAlbum(this FullAlbum album)
    {
        return new Spotify.Shared.DAL.Album.Models.Album(
            album.Id,
            album.Name,
            album.ReleaseDate,
            album.Images.FirstOrDefault()?.Url,
            album.Id,
            album.Name,
            album.Images.FirstOrDefault()?.Url,
            album.AlbumType
        );
    }
}