using Spotify.Shared.DAL.RefreshToken.Models;

namespace Spotify.Shared.DAL.RefreshToken;

public interface IRefreshTokenRepository
{
    public Task<RefreshToken.Models.RefreshToken?> FindByUserId(string userId);
    public Task<RefreshToken.Models.RefreshToken?> FindByToken(string tokenString);
    public Task<RefreshToken.Models.RefreshToken?> UpdateAsync(string id, UpdateRefreshToken token);
    public Task<RefreshToken.Models.RefreshToken> CreateAsync(RefreshToken.Models.CreateRefreshToken token);
    public Task DeleteAllTokensByUserId(string userId);
    public Task Delete(string refreshToken);
}