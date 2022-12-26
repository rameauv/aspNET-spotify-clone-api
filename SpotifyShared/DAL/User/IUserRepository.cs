using Spotify.Shared.DAL.User.Models;

namespace Spotify.Shared.DAL.User;

public interface IUserRepository
{
    Task<Models.User> GetAsync(string id);

    Task SetUser(string id, SetUserRequest userRequest);
}