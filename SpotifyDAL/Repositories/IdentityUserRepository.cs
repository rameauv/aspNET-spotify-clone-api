using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Repositories.Contexts;
using Spotify.Shared.DAL.IdentityUser;
using Spotify.Shared.DAL.IdentityUser.Models;

namespace Repositories.Repositories;

public class IdentityUserRepository : IIdentityUserRepository
{
    private bool _disposed = false;

    public IdentityUserRepository(IConfiguration configuration)
    {
        var optionsBuilder =
            new DbContextOptionsBuilder<SpotifyContext>().UseNpgsql(
                configuration.GetConnectionString("DBContext"));
        this.Context = new SpotifyContext(optionsBuilder.Options);
    }

    private SpotifyContext Context { get; }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                Context?.Dispose();
            }
        }

        this._disposed = true;
    }

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    public void UpdateById(string id, UpdateUser user)
    {
        using var dbContextTransaction = this.Context.Database.BeginTransaction();
        var entity = this.Context.Users.First(a => a.Id.ToString() == id);
        Context.Users.Update(entity);
        this.Context.SaveChanges();
        dbContextTransaction.Commit();
    }

    public async Task<IdentityUser?> FindByUserNameWithHashedPasswordAsync(string userName)
    {
        var entity = await this.Context.Users.FirstOrDefaultAsync(a => a.UserName == userName);
        if (entity == null)
        {
            return null;
        }

        return new IdentityUser(entity.Id, entity.UserName ?? "")
        {
            PasswordHash = entity.PasswordHash
        };
    }

    public async Task<IdentityUser?> FindByUserNameAsync(string userName)
    {
        var entity = await this.Context.Users.FirstOrDefaultAsync(a => a.UserName == userName);
        if (entity == null)
        {
            return null;
        }

        return new IdentityUser(entity.Id, entity.UserName ?? "");
    }

    public async Task<IdentityUser?> GetAsync(Guid id)
    {
        var entity = await this.Context.Users.FirstOrDefaultAsync(a => a.Id == id);
        if (entity == null)
        {
            return null;
        }

        return new IdentityUser(entity.Id, entity.UserName ?? "");
    }

    public void DeleteById(string id)
    {
        using var dbContextTransaction = this.Context.Database.BeginTransaction();
        var entity = this.Context.Users.First(a => a.Id.ToString() == id);
        Console.WriteLine(Context.Users.Remove(entity));
        this.Context.SaveChanges();
        dbContextTransaction.Commit();
    }

    public async Task<IdentityUser> CreateAsync(CreateUser user)
    {
        await using var dbContextTransaction = await this.Context.Database.BeginTransactionAsync();
        var newUser = (await Context.Users.AddAsync(new User
        {
            UserName = user.UserName,
            PasswordHash = user.Password,
        })).Entity;
        await this.Context.SaveChangesAsync();
        await dbContextTransaction.CommitAsync();
        return new IdentityUser(newUser.Id, newUser.UserName ?? "");
    }
}