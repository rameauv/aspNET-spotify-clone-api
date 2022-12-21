namespace Spotify.Shared.DAL.Contracts;

public interface IRefreshTokenRepository
{
    public Task<RefreshToken?> FindByDeviceIdAndUserId(Guid deviceId, Guid userId);

    public Task<RefreshToken?> UpdateAsync(Guid id, UpdateRefreshToken token);

    public Task<RefreshToken> CreateAsync(RefreshToken token);

}