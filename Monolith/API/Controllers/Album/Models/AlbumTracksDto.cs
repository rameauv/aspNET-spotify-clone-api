
using Api.Controllers.Search.Models;
using Api.Controllers.Shared;

namespace Api.Controllers.Album.Models;

public class AlbumTracksDto : PaggingDto<AlbumTrackDto>
{
    public AlbumTracksDto(AlbumTrackDto[] items, int limit, int offset, int total) : base(items, limit, offset, total)
    {
    }
}