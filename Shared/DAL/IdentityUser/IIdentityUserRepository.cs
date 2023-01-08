using Spotify.Shared.DAL.IdentityUser.Models;

namespace Spotify.Shared.DAL.IdentityUser;

public interface IIdentityUserRepository : IDisposable
{
    public Task<Models.AuthUser> CreateAsync(CreateUser user);

    public void DeleteById(string id);

    public void UpdateById(string id, UpdateUser user);

    public Task<Models.AuthUser?> FindByUserNameWithHashedPasswordAsync(string userName);

    public Task<Models.AuthUser?> FindByUserNameAsync(string userName);

    public Task<Models.AuthUser?> GetAsync(string id);
}