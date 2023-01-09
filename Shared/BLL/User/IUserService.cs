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
    /// Gets the user associated with the specified access token.
    /// </summary>
    /// <param name="accessToken">The access token to get the associated user for.</param>
    /// <returns>The user associated with the specified access token.</returns>
    /// <exception cref="Exception">Thrown if the access token does not contain a user identifier.</exception>
    Task<Models.User> CurrentUserAsync(string accessToken);

    /// <summary>
    /// Sets the name of the user associated with the specified access token.
    /// </summary>
    /// <param name="accessToken">The access token of the user to set the name for.</param>
    /// <param name="name">The new name of the user.</param>
    /// <exception cref="Exception">Thrown if the access token does not contain a user identifier.</exception>
    Task SetName(string accessToken, string name);
}