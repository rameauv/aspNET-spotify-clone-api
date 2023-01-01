namespace Spotify.Shared.BLL.User;

public interface IUserService
{
    Task<Models.User> GetAsync(string id);
    Task<Models.User> CurrentUserAsync(string accessToken);

    Task SetName(string accessToken, string name);
}