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

    /// <summary>
    /// Initializes a new instance of the `UserService` class.
    /// </summary>
    /// <param name="userRepository">The repository for interacting with user data.</param>
    public UserService(IUserRepository userRepository)
    {
        this._userRepository = userRepository;
    }

    public async Task<User> GetAsync(string id)
    {
        var res = await _userRepository.GetAsync(id);
        return new User(res.Id, res.Username, res.Name);
    }

    public Task<User> CurrentUserAsync(string userId)
    {
        return GetAsync(userId);
    }

    public async Task SetName(string userId, string name)
    {
        await _userRepository.SetUser(userId, new SetUserRequest
        {
            Name = name
        });
    }
}