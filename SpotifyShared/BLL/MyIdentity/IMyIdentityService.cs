using Spotify.Shared.DAL;
using Spotify.Shared.MyIdentity.Models;

namespace Spotify.Shared.MyIdentity.Contracts;

public interface IMyIdentityService
{
    public Task<MyResult> Register(RegisterUser user);

    public Task<Token?> Login(LoginCredentials credentials);

    public Task<Token?> RefreshAccessToken(string refreshToken);
}