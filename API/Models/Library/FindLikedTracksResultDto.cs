namespace Api.Models.Library;

public class FindLikedTracksResultDto: PaggingDto<TrackLibraryItem>
{
    public FindLikedTracksResultDto(IEnumerable<TrackLibraryItem> items, int limit, int offset, int total) : base(items, limit, offset, total)
    {
    }
}