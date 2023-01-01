using Spotify.Shared.BLL.Jwt.Models;
using Spotify.Shared.BLL.MyIdentity.Models;

namespace Spotify.Shared.BLL.MyIdentity;

public interface IMyIdentityService
{
    public Task<MyResult> Register(RegisterUser user);

    public Task<Token?> Login(LoginCredentials credentials);
    public Task Logout(string refreshToken);

    public Task<Token?> RefreshAccessToken(string refreshToken);
}