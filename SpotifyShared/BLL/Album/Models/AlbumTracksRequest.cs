namespace Spotify.Shared.BLL.Album.Models;

public class AlbumTracksRequest
{
    public int? Limit { get; set; } = null;
    public int? Offset { get; set; } = null;
}