using Spotify.Shared.DAL.IdentityUser.Models;

namespace Spotify.Shared.DAL.IdentityUser;

public interface IIdentityUserRepository : IDisposable
{
    public Task<Models.IdentityUser> CreateAsync(CreateUser user);

    public void DeleteById(string id);

    public void UpdateById(string id, UpdateUser user);

    public Task<Models.IdentityUser?> FindByUserNameWithHashedPasswordAsync(string userName);

    public Task<Models.IdentityUser?> FindByUserNameAsync(string userName);

    public Task<Models.IdentityUser?> GetAsync(Guid id);
}