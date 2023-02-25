using Spotify.Shared.BLL.Library.Models;
using Spotify.Shared.BLL.Like.Models;
using Spotify.Shared.BLL.Models;
using Spotify.Shared.BLL.Shared.Items;

namespace Spotify.Shared.BLL.Library;

public interface ILibraryService
{
    public Task<Models.Library> GetAsync(string userId);
    public Task<LibraryItems> FindLibraryItemsAsync(string userId, FindLikesByUserIdOptions options);
    public Task<Pagging<LibraryItem<SimpleTrack>>> FindLikedTracksAsync(string userId, FindLikedTracksOptions options);
}