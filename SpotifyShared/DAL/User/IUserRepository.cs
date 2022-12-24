namespace Spotify.Shared.DAL.User;

public interface IUserRepository
{
    Task<Models.User> GetAsync(string id);
}