using Spotify.Shared.BLL.Artist;
using Spotify.Shared.BLL.Artist.Models;
using Spotify.Shared.DAL.Artist;

namespace Spotify.BLL.Services;

public class ArtistService : IArtistService
{
    private readonly IArtistRepository _artistRepository;

    public ArtistService(IArtistRepository artistRepository)
    {
        this._artistRepository = artistRepository;
    }

    public async Task<Artist> GetAsync(string id)
    {
        var res = await _artistRepository.GetAsync(id);

        return new Artist(
            res.Id,
            res.Name,
            res.ThumbnailUrl,
            true,
            res.MonthlyListeners
        );
    }
}