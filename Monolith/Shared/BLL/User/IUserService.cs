namespace Spotify.Shared.BLL.User;

/// <summary>
/// An interface for a service that provides user-related functionality.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Gets a user with the specified identifier.
    /// </summary>
    /// <param name="id">The identifier of the user to get.</param>
    /// <returns>The user with the specified identifier.</returns>
    Task<Models.User> GetAsync(string id);

    /// <summary>
    /// Gets the user associated with the specified user id.
    /// </summary>
    /// <param name="userId">The id to get the associated user for.</param>
    /// <returns>The user associated with the specified access token.</returns>
    /// <exception cref="Exception">Thrown if the access token does not contain a user identifier.</exception>
    Task<Models.User> CurrentUserAsync(string userId);

    /// <summary>
    /// Sets the name of the user associated with the specified user id.
    /// </summary>
    /// <param name="userId">The id of the user to set the name for.</param>
    /// <param name="name">The new name of the user.</param>
    /// <exception cref="Exception">Thrown if the access token does not contain a user identifier.</exception>
    Task SetName(string userId, string name);
}