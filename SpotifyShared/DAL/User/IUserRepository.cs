namespace Spotify.Shared.DAL.Contracts;

public interface IUserRepository : IDisposable
{
    public Task<MyUser> CreateAsync(CreateUser user);

    public void DeleteById(string id);

    public void UpdateById(string id, UpdateUser user);

    public Task<MyUser?> FindByUserNameWithHashedPasswordAsync(string userName);

    public Task<MyUser?> FindByUserNameAsync(string userName);
    
    public Task<MyUser?> FindByIdAsync(Guid id);
}