using Microsoft.Extensions.Logging;
using Moq;
using Spotify.BLL.Services;
using Spotify.Shared.BLL.Jwt;
using Spotify.Shared.BLL.MyIdentity.Models;
using Spotify.Shared.BLL.Password;
using Spotify.Shared.DAL.IdentityUser;
using Spotify.Shared.DAL.RefreshToken;
using Spotify.Shared.DAL.RefreshToken.Models;
using Spotify.Shared.tools;
using AuthUser = Spotify.Shared.DAL.IdentityUser.Models.AuthUser;
using SharedDal = Spotify.Shared.DAL;

namespace BLL.UnitTests.Auth;

public class Login
{
    [Fact]
    public async Task ShouldReturnANotNullValue()
    {
        var userId = "some user id";
        var username = "some username";
        var password = "some password";
        var passwordHash = "password hash";
        var newAccessToken = "some new access token";
        var newRefreshToken = "some new refresh token";
        var credentials = new LoginCredentials(username, password);
        var authUser = new Spotify.Shared.BLL.Jwt.Models.AuthUser(userId, username) { PasswordHash = passwordHash };
        var createRefreshToken = new CreateRefreshToken(userId, newRefreshToken);

        var jwtServiceMock = new MyMock<IJwtService>();
        var identityUserRepositoryMock = new MyMock<IIdentityUserRepository>();
        var refreshTokenRepositoryMock = new MyMock<IRefreshTokenRepository>();
        var passwordServiceMock = new MyMock<IPasswordService>();
        var loggerServiceMock = new Mock<ILogger<AuthService>>();

        identityUserRepositoryMock.Setup(service => service.FindByUserNameWithHashedPasswordAsync(username))
            .ReturnsAsync(new AuthUser(userId, username) { PasswordHash = passwordHash });
        passwordServiceMock.Setup(service => service.Verify(password, passwordHash)).Returns(true);
        refreshTokenRepositoryMock.Setup(service => service.CreateAsync(createRefreshToken))
            .ReturnsAsync(new SharedDal.RefreshToken.Models.RefreshToken("refresh token id", userId, newRefreshToken));
        jwtServiceMock.Setup(service => service.GenerateAccessToken(authUser)).Returns(newAccessToken);
        jwtServiceMock.Setup(service => service.GenerateRefreshToken(authUser)).Returns(newRefreshToken);

        var authService = new AuthService(
            identityUserRepositoryMock.Object,
            refreshTokenRepositoryMock.Object,
            jwtServiceMock.Object,
            passwordServiceMock.Object,
            loggerServiceMock.Object
        );

        var res = await authService.Login(credentials);

        identityUserRepositoryMock.Verify(service => service.FindByUserNameWithHashedPasswordAsync(username), Times.Once);
        passwordServiceMock.Verify(service => service.Verify(password, passwordHash), Times.Once);
        jwtServiceMock.Verify(
            service => service.GenerateAccessToken(
                It.Is<Spotify.Shared.BLL.Jwt.Models.AuthUser>((user) => user == authUser)),
            Times.Once);
        jwtServiceMock.Verify(
            service => service.GenerateRefreshToken(
                It.Is<Spotify.Shared.BLL.Jwt.Models.AuthUser>((user) => user == authUser)),
            Times.Once);
        refreshTokenRepositoryMock.Verify(service => service.CreateAsync(createRefreshToken), Times.Once);

        Assert.NotNull(res);
    }

    [Fact]
    public async Task ShouldReturnANotEmptyNewAccessAndRefreshTokens()
    {
        var userId = "some user id";
        var username = "some username";
        var password = "some password";
        var passwordHash = "password hash";
        var newAccessToken = "some new access token";
        var newRefreshToken = "some new refresh token";
        var credentials = new LoginCredentials(username, password);
        var authUser = new Spotify.Shared.BLL.Jwt.Models.AuthUser(userId, username) { PasswordHash = passwordHash };
        var createRefreshToken = new CreateRefreshToken(userId, newRefreshToken);

        var jwtServiceMock = new MyMock<IJwtService>();
        var identityUserRepositoryMock = new MyMock<IIdentityUserRepository>();
        var refreshTokenRepositoryMock = new MyMock<IRefreshTokenRepository>();
        var passwordServiceMock = new MyMock<IPasswordService>();
        var loggerServiceMock = new Mock<ILogger<AuthService>>();

        identityUserRepositoryMock.Setup(service => service.FindByUserNameWithHashedPasswordAsync(username))
            .ReturnsAsync(new AuthUser(userId, username) { PasswordHash = passwordHash });
        passwordServiceMock.Setup(service => service.Verify(password, passwordHash)).Returns(true);
        refreshTokenRepositoryMock.Setup(service => service.CreateAsync(createRefreshToken))
            .ReturnsAsync(new SharedDal.RefreshToken.Models.RefreshToken("refresh token id", userId, newRefreshToken));

        jwtServiceMock.Setup(service => service.GenerateAccessToken(authUser)).Returns(newAccessToken);
        jwtServiceMock.Setup(service => service.GenerateRefreshToken(authUser)).Returns(newRefreshToken);

        var authService = new AuthService(
            identityUserRepositoryMock.Object,
            refreshTokenRepositoryMock.Object,
            jwtServiceMock.Object,
            passwordServiceMock.Object,
            loggerServiceMock.Object
        );

        var res = await authService.Login(credentials);

        identityUserRepositoryMock.Verify(service => service.FindByUserNameWithHashedPasswordAsync(username), Times.Once);
        passwordServiceMock.Verify(service => service.Verify(password, passwordHash), Times.Once);
        jwtServiceMock.Verify(
            service => service.GenerateAccessToken(
                It.Is<Spotify.Shared.BLL.Jwt.Models.AuthUser>((user) => user == authUser)),
            Times.Once);
        jwtServiceMock.Verify(
            service => service.GenerateRefreshToken(
                It.Is<Spotify.Shared.BLL.Jwt.Models.AuthUser>((user) => user == authUser)),
            Times.Once);
        refreshTokenRepositoryMock.Verify(service => service.CreateAsync(createRefreshToken), Times.Once);

        Assert.NotNull(res?.AccessToken);
        Assert.NotNull(res?.RefreshToken);
        Assert.NotEqual(res?.RefreshToken, string.Empty);
        Assert.NotEqual(res?.AccessToken, string.Empty);
    }


    [Fact]
    public async Task ShouldReturnNullIfTheUserIsNotFound()
    {
        var username = "some username";
        var password = "some password";
        var credentials = new LoginCredentials(username, password);

        var jwtServiceMock = new MyMock<IJwtService>();
        var identityUserRepository = new MyMock<IIdentityUserRepository>();
        var refreshTokenRepository = new MyMock<IRefreshTokenRepository>();
        var passwordServiceMock = new MyMock<IPasswordService>();
        var loggerServiceMock = new Mock<ILogger<AuthService>>();

        identityUserRepository.Setup(service => service.FindByUserNameWithHashedPasswordAsync(username))
            .ReturnsAsync((AuthUser?)null);

        var authService = new AuthService(
            identityUserRepository.Object,
            refreshTokenRepository.Object,
            jwtServiceMock.Object,
            passwordServiceMock.Object,
            loggerServiceMock.Object
        );

        var res = await authService.Login(credentials);

        identityUserRepository.Verify(service => service.FindByUserNameWithHashedPasswordAsync(username), Times.Once);
        Assert.Null(res);
    }

    [Fact]
    public async Task ShouldReturnNullIfThePasswordDoesNotMatchThePasswordHash()
    {
        var username = "some username";
        var password = "some password";
        var passwordHash = "password hash";
        var userId = "some user id";
        var credentials = new LoginCredentials(username, password);

        var jwtServiceMock = new MyMock<IJwtService>();
        var identityUserRepository = new MyMock<IIdentityUserRepository>();
        var refreshTokenRepository = new MyMock<IRefreshTokenRepository>();
        var passwordServiceMock = new MyMock<IPasswordService>();
        var loggerServiceMock = new Mock<ILogger<AuthService>>();

        identityUserRepository.Setup(service => service.FindByUserNameWithHashedPasswordAsync(username))
            .ReturnsAsync(new AuthUser(userId, username) { PasswordHash = passwordHash });
        passwordServiceMock.Setup(service => service.Verify(password, passwordHash)).Returns(false);

        var authService = new AuthService(
            identityUserRepository.Object,
            refreshTokenRepository.Object,
            jwtServiceMock.Object,
            passwordServiceMock.Object,
            loggerServiceMock.Object
        );

        var res = await authService.Login(credentials);
        identityUserRepository.Verify(service => service.FindByUserNameWithHashedPasswordAsync(username), Times.Once);
        passwordServiceMock.Verify(service => service.Verify(password, passwordHash), Times.Once);
        Assert.Null(res);
    }
}