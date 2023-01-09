using Spotify.Shared.BLL.Jwt.Models;
using Spotify.Shared.BLL.MyIdentity.Models;

namespace Spotify.Shared.BLL.MyIdentity;

/// <summary>
/// Provides authentication and authorization functionality.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Registers a new user with the provided username and password.
    /// </summary>
    /// <param name="user">The user to register.</param>
    /// <returns>A MyResult object indicating the success or failure of the operation.</returns>
    /// <remarks> 
    /// Many of the exceptions listed above are not thrown directly from this method. See <see cref="Validators"/> to examine the call graph.
    /// </remarks>
    public Task Register(RegisterUser user);

    /// <summary>
    /// Logs in a user with the provided login credentials.
    /// If the provided credentials are valid, returns a Token object containing an access token and refresh token.
    /// If the provided credentials are invalid, returns null.
    /// </summary>
    /// <param name="credentials">The login credentials of the user.</param>
    /// <returns>A Token object containing an access token and refresh token, or null if the provided credentials are invalid.</returns>
    public Task<Token?> Login(LoginCredentials credentials);

    /// <summary>
    /// Logs out a user by deleting the provided refresh token.
    /// </summary>
    /// <param name="refreshToken">The refresh token to delete.</param>
    public Task Logout(string refreshToken);

    /// <summary>
    /// Refreshes an access token using the provided refresh token.
    /// If the provided refresh token is valid, returns a Token object containing a new access token and refresh token.
    /// If the provided refresh token is invalid, returns null.
    /// </summary>
    /// <param name="refreshToken">The refresh token to use for refreshing the access token.</param>
    /// <returns>A Token object containing a new access token and refresh token, or null if the provided refresh token is invalid.</returns>
    public Task<Token?> RefreshAccessToken(string refreshToken);
}