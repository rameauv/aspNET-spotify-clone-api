using Spotify.Shared.DAL.Album.Models;
using Spotify.Shared.DAL.Models;

namespace Spotify.Shared.DAL.Like.models;

public class UserLikes: Pagging<Like>
{
    public UserLikes(IEnumerable<Like> items, int limit, int offset, int total) : base(items, limit, offset, total)
    {
    }
}