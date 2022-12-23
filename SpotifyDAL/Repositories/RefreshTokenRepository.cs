using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Repositories.Contexts;
using Spotify.Shared.DAL.Contracts;
using SharedDal = Spotify.Shared.DAL;

namespace Repositories.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private Contexts.SpotifyContext Context { get; }

    public RefreshTokenRepository(IConfiguration configuration)
    {
        var optionsBuilder =
            new DbContextOptionsBuilder<Contexts.SpotifyContext>().UseNpgsql(configuration.GetConnectionString("DBContext"));
        Context = new Contexts.SpotifyContext(optionsBuilder.Options);
    }

    ~RefreshTokenRepository()
    {
        Context.Dispose();
    }

    public async Task<SharedDal.RefreshToken?> FindByDeviceIdAndUserId(Guid deviceId, Guid userId)
    {
        var token = await Context.RefreshTokens
            .FirstOrDefaultAsync(refreshToken => refreshToken.DeviceId == deviceId && refreshToken.UserId == userId);
        if (token == null)
        {
            return null;
        }

        return new SharedDal.RefreshToken(token.UserId, token.DeviceId, token.Token);
    }

    public async Task<SharedDal.RefreshToken?> UpdateAsync(Guid id, SharedDal.UpdateRefreshToken token)
    {
        await using var dbContextTransaction = await Context.Database.BeginTransactionAsync();
        var entity = await Context.RefreshTokens.FirstAsync(refreshToken => refreshToken.DeviceId == id);
        if (token.Token.HasValue && token.Token.Value != null)
        {
            entity.Token = token.Token.Value;
        }

        Context.RefreshTokens.Update(entity);
        await dbContextTransaction.CommitAsync();
        return new SharedDal.RefreshToken(
            entity.UserId,
            entity.DeviceId,
            entity.Token
        );
    }

    public async Task<SharedDal.RefreshToken> CreateAsync(SharedDal.RefreshToken token)
    {
        await using var dbContextTransaction = await Context.Database.BeginTransactionAsync();
        var newUser = (await Context.RefreshTokens.AddAsync(new RefreshToken{
            UserId = token.UserId,
            DeviceId = token.DeviceId,
            Token = token.Token
        })).Entity;
        await Context.SaveChangesAsync();
        await dbContextTransaction.CommitAsync();
        return new SharedDal.RefreshToken(
            newUser.UserId,
            token.DeviceId,
            token.Token
        );
    }
}