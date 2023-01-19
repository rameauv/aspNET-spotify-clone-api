using Microsoft.Extensions.Logging;
using Moq;
using Spotify.BLL.Services;
using Spotify.Shared.BLL.Jwt;
using Spotify.Shared.BLL.MyIdentity.Models;
using Spotify.Shared.BLL.Password;
using Spotify.Shared.DAL.IdentityUser;
using Spotify.Shared.DAL.IdentityUser.Models;
using Spotify.Shared.DAL.RefreshToken;
using Spotify.Shared.DAL.User.Models;
using Spotify.Shared.tools;

namespace BLL.UnitTests.Auth;

public class Register
{
    [Fact]
    public async Task ShouldRegister()
    {
        var userId = "some user id";
        var username = "some username";
        var password = "some password";
        var passwordHash = "some fake hash";
        var data = "some data";
        var registrationUser = new RegisterUser(username, password, data);
        var createUser = new CreateUser(username, passwordHash, new UserData(username));
        var identityUser = new AuthUser(userId, username)
        {
            PasswordHash = passwordHash
        };

        var jwtServiceMock = new MyMock<IJwtService>();
        var identityUserRepositoryMock = new MyMock<IIdentityUserRepository>();
        var refreshTokenRepositoryMock = new MyMock<IRefreshTokenRepository>();
        var passwordServiceMock = new MyMock<IPasswordService>();
        var loggerServiceMock = new Mock<ILogger<AuthService>>();

        passwordServiceMock.Setup(service => service.Hash(password)).Returns(passwordHash);
        identityUserRepositoryMock.Setup(service => service.CreateAsync(createUser))
            .ReturnsAsync(identityUser);

        var authService = new AuthService(
            identityUserRepositoryMock.Object,
            refreshTokenRepositoryMock.Object,
            jwtServiceMock.Object,
            passwordServiceMock.Object,
            loggerServiceMock.Object
        );

        await authService.Register(registrationUser);

        passwordServiceMock.Verify(service => service.Hash(password), Times.Once);
        identityUserRepositoryMock
            .Verify(service => service.CreateAsync(createUser),
                Times.Once);
    }
}