using Api.Controllers.Shared.Items.Models;
using Spotify.Shared.BLL.Shared.Items;

namespace Api.Controllers.Shared.Items.Extensions;

public static class SimpleTrackDtoMappingExtension
{
    public static SimpleTrackDto ToDto(this SimpleTrack track)
    {
        return new SimpleTrackDto(
            track.Id,
            track.ThumbnailUrl,
            track.Title,
            track.ArtistName
        );
    }
}