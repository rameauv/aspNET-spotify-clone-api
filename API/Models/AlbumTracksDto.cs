
namespace Api.Models;

public class AlbumTracksDto : PaggingDto<SimpleTrackDto>
{
    public AlbumTracksDto(SimpleTrackDto[] items, int limit, int offset, int total) : base(items, limit, offset, total)
    {
    }
}