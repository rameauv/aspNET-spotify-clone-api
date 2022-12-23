using Spotify.Shared.BLL.Track;
using Spotify.Shared.BLL.Track.Models;
using Spotify.Shared.DAL.Track;

namespace Spotify.BLL.Services;

public class TrackService : ITrackService
{
    private readonly ITrackRepository _trackRepository;

    public TrackService(ITrackRepository trackRepository)
    {
        _trackRepository = trackRepository;
    }


    public async Task<Track> GetAsync(string id)
    {
        var res = await _trackRepository.GetAsync(id);

        return new Track(
            res.Id,
            res.Title,
            res.ArtistName,
            res.ThumbnailUrl,
            true
        );
    }
}