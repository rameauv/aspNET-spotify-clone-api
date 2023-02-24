using AutoMapper;
using Spotify.Shared.BLL.Like.Models;
using Spotify.Shared.BLL.Track;
using Spotify.Shared.BLL.Track.Models;
using Spotify.Shared.DAL.Like;
using Spotify.Shared.DAL.Track;

namespace Spotify.BLL.Services;

/// <summary>
/// A service for interacting with tracks.
/// </summary>
public class TrackService : ITrackService
{
    private readonly ITrackRepository _trackRepository;
    private readonly ILikeRepository _likeRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="TrackService"/> class.
    /// </summary>
    /// <param name="trackRepository">The repository for interacting with tracks.</param>
    /// <param name="likeRepository">The repository for interacting with likes.</param>
    public TrackService(
        ITrackRepository trackRepository,
        ILikeRepository likeRepository,
        IMapper mapper
    )
    {
        _trackRepository = trackRepository;
        _likeRepository = likeRepository;
        _mapper = mapper;
    }

    public async Task<Track?> GetAsync(string id, string userId)
    {
        var trackTask = _trackRepository.GetAsync(id);
        var likeTask = _likeRepository.GetByAssociatedUserIdAsync(id, userId);
        await Task.WhenAll(trackTask, likeTask);
        var track = await trackTask;
        if (track == null)
        {
            return null;
        }

        var like = await likeTask;

        return new Track(
            track.Id,
            track.Title,
            track.ArtistName,
            track.ThumbnailUrl,
            like?.Id
        );
    }

    public async Task<Like> SetLikeAsync(string id, string userId)
    {
        var like = await _likeRepository.SetAsync(id, "track", userId);
        var associatedType = _mapper.Map<AssociatedType>(like.AssociatedType);
        return new Like(like.Id, like.AssociatedId, like.AssociatedUser, associatedType);
    }
}