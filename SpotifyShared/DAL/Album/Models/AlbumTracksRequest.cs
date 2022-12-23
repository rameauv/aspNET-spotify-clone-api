namespace Spotify.Shared.DAL.Album.Models;

public class AlbumTracksRequest
{
    public int? Limit { get; set; } = null;
    public int? Offset { get; set; } = null;
}