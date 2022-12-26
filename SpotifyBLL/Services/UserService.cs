using System.Security.Claims;
using Spotify.Shared.BLL.MyIdentity;
using Spotify.Shared.BLL.User;
using Spotify.Shared.DAL.User;
using Spotify.Shared.DAL.User.Models;
using User = Spotify.Shared.BLL.User.Models.User;

namespace Spotify.BLL.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMyIdentityService _identityService;

    public UserService(IUserRepository userRepository, IMyIdentityService identityService)
    {
        this._userRepository = userRepository;
        this._identityService = identityService;
    }

    public async Task<User> GetAsync(string id)
    {
        var res = await _userRepository.GetAsync(id);
        return new User(res.Id, res.Username, res.Name);
    }

    public Task<User> CurrentUserAsync(string accessToken)
    {
        var validatedToken = _identityService.GetSecurityToken(accessToken);
        var userId = validatedToken.Principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            throw new Exception("no userid in this access token");
        }

        return GetAsync(userId);
    }

    public async Task SetName(string accessToken, string name)
    {
        var validatedToken = _identityService.GetSecurityToken(accessToken);
        var userId = validatedToken.Principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            throw new Exception("no userid in this access token");
        }

        await _userRepository.SetUser(userId, new SetUserRequest
        {
            Name = name
        });
    }
}