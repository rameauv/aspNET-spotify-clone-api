using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Repositories.Contexts;
using Spotify.Shared.DAL.User;
using Spotify.Shared.DAL.User.Models;
using UserData = Repositories.Repositories.User.Models.UserData;

namespace Repositories.Repositories.User;

public class UserRepository : IUserRepository
{
    private SpotifyContext Context { get; }

    public UserRepository(IConfiguration configuration)
    {
        var optionsBuilder =
            new DbContextOptionsBuilder<SpotifyContext>().UseNpgsql(
                configuration.GetConnectionString("DBContext"));
        Context = new SpotifyContext(optionsBuilder.Options);
    }

    ~UserRepository()
    {
        Context.Dispose();
    }

    public async Task<Spotify.Shared.DAL.User.Models.User> GetAsync(string id)
    {
        var res = await Context.Users
            .Where(user => user.Id == new Guid(id))
            .Select(user => new {id = user.Id.ToString(), userName = user.UserName, data = user.Data})
            .FirstAsync();
        var data = JsonSerializer.Deserialize<UserData>(res.data ?? "{}")
                   ?? new UserData("");
        
        return new Spotify.Shared.DAL.User.Models.User(res.id, res.userName, data.Name);
    }

    public async Task SetUser(string id, SetUserRequest userRequest)
    {
        var entity = await Context.Users.FindAsync(new Guid(id));
        if (entity == null)
        {
            throw new Exception("entity not found");
        }

        var data = JsonSerializer.Deserialize<UserData>(entity.Data ?? "{}")
                   ?? new UserData("");
        if (userRequest.Name.HasValue)
        {
            data.Name = userRequest.Name.Value;
        }

        entity.Data = JsonSerializer.Serialize(data);
        Context.Users.Update(entity);
        await Context.SaveChangesAsync();
    }
}