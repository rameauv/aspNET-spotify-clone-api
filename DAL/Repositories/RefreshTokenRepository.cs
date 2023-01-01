using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Spotify.Shared.DAL.RefreshToken;
using Spotify.Shared.DAL.RefreshToken.Models;
using RefreshToken = Repositories.Contexts.RefreshToken;
using SharedDal = Spotify.Shared.DAL;

namespace Repositories.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private Contexts.SpotifyContext Context { get; }

    public RefreshTokenRepository(IConfiguration configuration)
    {
        var optionsBuilder =
            new DbContextOptionsBuilder<Contexts.SpotifyContext>().UseNpgsql(
                configuration.GetConnectionString("DBContext"));
        Context = new Contexts.SpotifyContext(optionsBuilder.Options);
    }

    ~RefreshTokenRepository()
    {
        Context.Dispose();
    }

    public async Task<Spotify.Shared.DAL.RefreshToken.Models.RefreshToken?> FindByUserId(string userId)
    {
        var token = await Context.RefreshTokens
            .FirstOrDefaultAsync(refreshToken => refreshToken.UserId == userId);
        if (token == null)
        {
            return null;
        }

        return new Spotify.Shared.DAL.RefreshToken.Models.RefreshToken(token.Id, token.UserId, token.Token);
    }

    public async Task<Spotify.Shared.DAL.RefreshToken.Models.RefreshToken?> FindByToken(string tokenString)
    {
        var savedToken = await Context.RefreshTokens
            .FirstOrDefaultAsync(refreshToken => refreshToken.Token == tokenString);
        if (savedToken == null)
        {
            return null;
        }

        return new Spotify.Shared.DAL.RefreshToken.Models.RefreshToken(savedToken.Id, savedToken.UserId, savedToken.Token);
    }

    public async Task<Spotify.Shared.DAL.RefreshToken.Models.RefreshToken?> UpdateAsync(string id,
        UpdateRefreshToken token)
    {
        await using var dbContextTransaction = await Context.Database.BeginTransactionAsync();
        var entity = await Context.RefreshTokens.FirstAsync(refreshToken => refreshToken.Id == id);
        if (token.Token.HasValue && token.Token.Value != null)
        {
            entity.Token = token.Token.Value;
        }

        Context.RefreshTokens.Update(entity);
        await Context.SaveChangesAsync();
        await dbContextTransaction.CommitAsync();
        return new Spotify.Shared.DAL.RefreshToken.Models.RefreshToken(
            entity.Id,
            entity.UserId,
            entity.Token
        );
    }

    public async Task<Spotify.Shared.DAL.RefreshToken.Models.RefreshToken> CreateAsync(
        Spotify.Shared.DAL.RefreshToken.Models.CreateRefreshToken token)
    {
        await using var dbContextTransaction = await Context.Database.BeginTransactionAsync();
        var newToken = (await Context.RefreshTokens.AddAsync(new RefreshToken
        {
            UserId = token.UserId,
            Token = token.Token
        })).Entity;
        await Context.SaveChangesAsync();
        await dbContextTransaction.CommitAsync();
        return new Spotify.Shared.DAL.RefreshToken.Models.RefreshToken(
            newToken.Id,
            newToken.UserId,
            newToken.Token
        );
    }

    public async Task DeleteAllTokensByUserId(string userId)
    {
        var entities = Context.RefreshTokens.Where(token => token.UserId == userId);
        Context.RefreshTokens.RemoveRange(entities);
        await Context.SaveChangesAsync();
    }

    public async Task Delete(string refreshToken)
    {
        var queryable = Context.RefreshTokens.Where(token => token.Token == refreshToken).Take(1);
        Context.RefreshTokens.RemoveRange(queryable);
        await Context.SaveChangesAsync();
    }
}