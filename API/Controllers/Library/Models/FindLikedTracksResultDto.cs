using Api.Controllers.Shared;
using Api.Controllers.Shared.Items.Models;

namespace Api.Controllers.Library.Models;

public class FindLikedTracksResultDto : PaggingDto<LibraryItemDto<SimpleTrackDto>>
{
    public FindLikedTracksResultDto(IEnumerable<LibraryItemDto<SimpleTrackDto>> items, int limit, int offset, int total)
        : base(items, limit, offset, total)
    {
    }
}