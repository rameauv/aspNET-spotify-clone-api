using Spotify.Shared.BLL.Models;
using Spotify.Shared.BLL.Shared.Items;

namespace Spotify.Shared.BLL.Album.Models;

public class AlbumTracks : Pagging<AlbumTrack>
{
    public AlbumTracks(AlbumTrack[] items, int limit = 0, int offset = 0, int total = 0)
        : base(items, limit, offset, total)
    {
    }
}