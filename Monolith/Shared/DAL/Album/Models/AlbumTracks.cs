using Spotify.Shared.DAL.Shared;

namespace Spotify.Shared.DAL.Album.Models;

public class AlbumTracks : Pagging<SimpleTrack>
{
    public AlbumTracks(IEnumerable<SimpleTrack> items, int limit = 0, int offset = 0, int total = 0)
        : base(items, limit, offset, total)
    {
    }
}