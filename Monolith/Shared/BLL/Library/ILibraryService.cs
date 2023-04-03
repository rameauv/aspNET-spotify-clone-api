using Spotify.Shared.BLL.Library.Models;
using Spotify.Shared.BLL.Like.Models;
using Spotify.Shared.BLL.Models;
using Spotify.Shared.BLL.Shared.Items;

namespace Spotify.Shared.BLL.Library;

/// <summary>
/// Provides methods for interacting with the library.
/// </summary>
public interface ILibraryService
{
    /// <summary>
    /// Get the a user's library
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <returns>A new intance of <see cref="Models.Library"/></returns>
    public Task<Models.Library> GetAsync(string userId);

    /// <summary>
    /// Find a user's library items
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <param name="options">Options for filtering</param>
    /// <returns>A new instance of <see cref="LibraryItems"/>"/></returns>
    public Task<LibraryItems> FindLibraryItemsAsync(string userId, FindLikesByUserIdOptions options);
    /// <summary>
    /// Find a user's liked songs
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <param name="options">Options for filtering</param>
    /// <returns>The liked songs</returns>
    public Task<Pagging<LibraryItem<SimpleTrack>>> FindLikedTracksAsync(string userId, FindLikedTracksOptions options);
}