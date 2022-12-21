using Spotify.Models.BLL.Contracts;
using Spotify.Shared.DAL.Contracts;
using SharedDal = Spotify.Shared.DAL;

namespace Spotify.BLL.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        this._userRepository = userRepository;
    }
    
}