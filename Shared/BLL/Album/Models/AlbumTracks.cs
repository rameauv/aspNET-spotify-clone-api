using Spotify.Shared.BLL.Models;

namespace Spotify.Shared.BLL.Album.Models;

public class AlbumTracks : Pagging<SimpleTrack>
{
    public AlbumTracks(SimpleTrack[] items, int limit = 0, int offset = 0, int total = 0)
        : base(items, limit, offset, total)
    {
    }
}