using Api.Models.Items;

namespace Api.Models.Library;

public class FindLikedTracksResultDto : PaggingDto<LibraryItemDto<SimpleTrack>>
{
    public FindLikedTracksResultDto(IEnumerable<LibraryItemDto<SimpleTrack>> items, int limit, int offset, int total)
        : base(items, limit, offset, total)
    {
    }
}