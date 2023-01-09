using System.Security.Claims;
using Spotify.Shared.BLL.Jwt;
using Spotify.Shared.BLL.User;
using Spotify.Shared.DAL.User;
using Spotify.Shared.DAL.User.Models;
using User = Spotify.Shared.BLL.User.Models.User;

namespace Spotify.BLL.Services;

/// <summary>
/// Service class for managing user data.
/// </summary>
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;

    /// <summary>
    /// Initializes a new instance of the `UserService` class.
    /// </summary>
    /// <param name="userRepository">The repository for interacting with user data.</param>
    /// <param name="jwtService">The service for handling JSON Web Tokens (JWTs).</param>
    public UserService(IUserRepository userRepository, IJwtService jwtService)
    {
        this._userRepository = userRepository;
        this._jwtService = jwtService;
    }

    /// <summary>
    /// Gets a user with the specified identifier.
    /// </summary>
    /// <param name="id">The identifier of the user to get.</param>
    /// <returns>The user with the specified identifier.</returns>
    public async Task<User> GetAsync(string id)
    {
        var res = await _userRepository.GetAsync(id);
        return new User(res.Id, res.Username, res.Name);
    }

    /// <summary>
    /// Gets the user associated with the specified access token.
    /// </summary>
    /// <param name="accessToken">The access token to get the associated user for.</param>
    /// <returns>The user associated with the specified access token.</returns>
    /// <exception cref="Exception">Thrown if the access token does not contain a user identifier.</exception>
    public Task<User> CurrentUserAsync(string accessToken)
    {
        var validatedToken = _jwtService.GetValidatedAccessToken(accessToken);
        var userId = validatedToken.Principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            throw new Exception("no userid in this access token");
        }

        return GetAsync(userId);
    }

    /// <summary>
    /// Sets the name of the user associated with the specified access token.
    /// </summary>
    /// <param name="accessToken">The access token of the user to set the name for.</param>
    /// <param name="name">The new name of the user.</param>
    /// <exception cref="Exception">Thrown if the access token does not contain a user identifier.</exception>
    public async Task SetName(string accessToken, string name)
    {
        var validatedToken = _jwtService.GetValidatedAccessToken(accessToken);
        var userId = validatedToken.Principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            throw new Exception("no userid in this access token");
        }

        await _userRepository.SetUser(userId, new SetUserRequest
        {
            Name = name
        });
    }
}