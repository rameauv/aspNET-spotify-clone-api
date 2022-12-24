using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Spotify.Shared.DAL.User;
using Spotify.Shared.DAL.User.Models;

namespace Repositories.Repositories;

public class UserRepository : IUserRepository
{
    private Contexts.SpotifyContext Context { get; }

    public UserRepository(IConfiguration configuration)
    {
        var optionsBuilder =
            new DbContextOptionsBuilder<Contexts.SpotifyContext>().UseNpgsql(
                configuration.GetConnectionString("DBContext"));
        Context = new Contexts.SpotifyContext(optionsBuilder.Options);
    }

    ~UserRepository()
    {
        Context.Dispose();
    }

    public async Task<User> GetAsync(string id)
    {
        var res = await Context.Users
            .Where(user => user.Id == new Guid(id))
            .Select(user => new User(user.Id.ToString(), user.UserName, "toto"))
            .FirstAsync();
        return res;
    }
}