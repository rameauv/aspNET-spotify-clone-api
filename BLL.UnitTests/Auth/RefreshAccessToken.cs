using Microsoft.IdentityModel.Tokens;
using Moq;
using Spotify.BLL.Services;
using Spotify.Shared.BLL.Jwt;
using Spotify.Shared.BLL.Jwt.Models;
using Spotify.Shared.BLL.Password;
using Spotify.Shared.DAL.IdentityUser;
using Spotify.Shared.DAL.RefreshToken;
using Spotify.Shared.DAL.RefreshToken.Models;
using SharedDal = Spotify.Shared.DAL;
using Spotify.Shared.tools;

namespace BLL.UnitTests.Auth;

public class RefreshAccessToken
{
    public RefreshAccessToken()
    {
    }

    [Fact]
    public async void ShouldReturnANotNullObject()
    {
        var fakeUserId = "fake user id";
        var fakeUserName = "fake username";
        var newRefreshToken = "fakeNewRefreshToken";
        var newAccessToken = "fakeNewAccessToken";
        var fakeRefreshTokenId = "fake refresh token id";
        var currentRefreshToken = "a current refresh token";
        var myUser = new Spotify.Shared.BLL.Jwt.Models.AuthUser(fakeUserId, fakeUserName);

        var identityUserRepositoryMock = new MyMock<IIdentityUserRepository>();
        var refreshTokenRepositoryMock = new MyMock<IRefreshTokenRepository>();
        var jwtServiceMock = new MyMock<IJwtService>();
        var passwordServiceMock = new MyMock<IPasswordService>();

        jwtServiceMock.Setup(service => service.GetJwtTokenContent(currentRefreshToken))
            .Returns(new JwtTokenContent(fakeUserId));
        jwtServiceMock.Setup(service => service.GenerateRefreshToken(myUser))
            .Returns(newRefreshToken);
        jwtServiceMock.Setup(service => service.GenerateAccessToken(myUser))
            .Returns(newAccessToken);
        refreshTokenRepositoryMock.Setup(service => service.FindByToken(currentRefreshToken))
            .ReturnsAsync(new RefreshToken(fakeRefreshTokenId, fakeUserId, currentRefreshToken));
        identityUserRepositoryMock.Setup(service => service.GetAsync(fakeUserId))
            .ReturnsAsync(new SharedDal.IdentityUser.Models.AuthUser(fakeUserId, fakeUserName));
        refreshTokenRepositoryMock.Setup(service =>
                service.UpdateAsync(fakeRefreshTokenId, new UpdateRefreshToken() { Token = newRefreshToken }))
            .ReturnsAsync(new RefreshToken(fakeRefreshTokenId, fakeUserId, newRefreshToken));

        var identityService =
            new AuthService(
                identityUserRepositoryMock.Object,
                refreshTokenRepositoryMock.Object,
                jwtServiceMock.Object,
                passwordServiceMock.Object
            );
        var res = await identityService.RefreshAccessToken(currentRefreshToken);
        jwtServiceMock.Verify(service =>
            service.GenerateRefreshToken(It.Is<Spotify.Shared.BLL.Jwt.Models.AuthUser>(b => b == myUser)));
        jwtServiceMock.Verify(service =>
            service.GenerateAccessToken(It.Is<Spotify.Shared.BLL.Jwt.Models.AuthUser>(b => b == myUser)));
        Assert.NotNull(res);
    }

    [Fact]
    public async void ShouldReturnANewAccessTokenAndANewRefreshToken()
    {
        var userId = "a fake user id";
        var username = "fake username";
        var currentRefreshToken = "current refresh token";
        var currentRefreshTokenId = "fake refresh token id";
        var newRefreshToken = "fakeNewRefreshToken";
        var newAccessToken = "fakeNewAccessToken";
        var myUser = new Spotify.Shared.BLL.Jwt.Models.AuthUser(userId, username);

        var identityUserRepositoryMock = new MyMock<IIdentityUserRepository>();
        var refreshTokenRepositoryMock = new MyMock<IRefreshTokenRepository>();
        var jwtServiceMock = new MyMock<IJwtService>();
        var passwordServiceMock = new MyMock<IPasswordService>();

        jwtServiceMock.Setup(service => service.GetJwtTokenContent(currentRefreshToken))
            .Returns(new JwtTokenContent(userId));
        jwtServiceMock.Setup(service => service.GenerateRefreshToken(myUser))
            .Returns(newRefreshToken);
        jwtServiceMock.Setup(service => service.GenerateAccessToken(myUser))
            .Returns(newAccessToken);
        refreshTokenRepositoryMock.Setup(service => service.FindByToken(currentRefreshToken))
            .ReturnsAsync(new RefreshToken(currentRefreshTokenId, userId, currentRefreshTokenId));
        identityUserRepositoryMock.Setup(service => service.GetAsync(userId))
            .ReturnsAsync(new SharedDal.IdentityUser.Models.AuthUser(userId, username));
        refreshTokenRepositoryMock.Setup(service =>
                service.UpdateAsync(currentRefreshTokenId, new UpdateRefreshToken() { Token = newRefreshToken }))
            .ReturnsAsync(new RefreshToken(currentRefreshTokenId, userId, newRefreshToken));

        var identityService =
            new AuthService(
                identityUserRepositoryMock.Object,
                refreshTokenRepositoryMock.Object,
                jwtServiceMock.Object,
                passwordServiceMock.Object
            );
        var res = await identityService.RefreshAccessToken(currentRefreshToken);

        jwtServiceMock.Verify(service =>
            service.GenerateRefreshToken(It.Is<Spotify.Shared.BLL.Jwt.Models.AuthUser>(b => b == myUser)));
        jwtServiceMock.Verify(service =>
            service.GenerateAccessToken(It.Is<Spotify.Shared.BLL.Jwt.Models.AuthUser>(b => b == myUser)));

        Assert.Equal(res?.RefreshToken, newRefreshToken);
        Assert.Equal(res?.AccessToken, newAccessToken);
    }

    [Fact]
    public async void ShouldReturnNullIfTheValidationServiceDetectAReplay()
    {
        var currentRefreshTokenId = "fake refresh token id";
        var currentRefreshToken = "current refresh token";
        var userId = "some fake user id";

        var identityUserRepositoryMock = new MyMock<IIdentityUserRepository>();
        var refreshTokenRepositoryMock = new MyMock<IRefreshTokenRepository>();
        var jwtServiceMock = new MyMock<IJwtService>();
        var passwordServiceMock = new MyMock<IPasswordService>();

        jwtServiceMock.Setup(service => service.GetJwtTokenContent(currentRefreshToken))
            .Returns(new JwtTokenContent(userId));
        jwtServiceMock.Setup(service => service.ValidateRefreshTokenToken(currentRefreshToken))
            .Throws<SecurityTokenReplayDetectedException>();
        refreshTokenRepositoryMock.Setup(service => service.FindByToken(currentRefreshToken))
            .ReturnsAsync(new RefreshToken(currentRefreshTokenId, userId, currentRefreshToken));
        refreshTokenRepositoryMock.Setup(service => service.DeleteAllTokensByUserId(userId))
            .Returns(Task.FromResult(true));

        var identityService =
            new AuthService(
                identityUserRepositoryMock.Object,
                refreshTokenRepositoryMock.Object,
                jwtServiceMock.Object,
                passwordServiceMock.Object
            );
        var res = await identityService.RefreshAccessToken(currentRefreshToken);
        Assert.Null(res);
    }

    [Fact]
    public async void ShouldRemoveAllUserSRefreshTokenIfTheValidationServiceDetectAReplay()
    {
        var currentRefreshToken = "current refresh token";
        var fakeUserId = "fake user id";

        var identityUserRepositoryMock = new MyMock<IIdentityUserRepository>();
        var refreshTokenRepositoryMock = new MyMock<IRefreshTokenRepository>();
        var jwtServiceMock = new MyMock<IJwtService>();
        var passwordServiceMock = new MyMock<IPasswordService>();

        jwtServiceMock.Setup(service => service.GetJwtTokenContent(currentRefreshToken))
            .Returns(new JwtTokenContent(fakeUserId));
        jwtServiceMock.Setup(service => service.ValidateRefreshTokenToken(currentRefreshToken))
            .Throws<SecurityTokenReplayDetectedException>();
        refreshTokenRepositoryMock.Setup(service => service.DeleteAllTokensByUserId(fakeUserId))
            .Returns(Task.FromResult(true));

        var identityService =
            new AuthService(
                identityUserRepositoryMock.Object,
                refreshTokenRepositoryMock.Object,
                jwtServiceMock.Object,
                passwordServiceMock.Object
            );
        await identityService.RefreshAccessToken(currentRefreshToken);
        refreshTokenRepositoryMock.Verify(service => service.DeleteAllTokensByUserId(fakeUserId), Times.Once);
    }

    [Fact]
    public async void ShouldReturnNullIfReplayAttackIsDetectedWithAdbCheck()
    {
        var currentRefreshToken = "current refresh token";
        var fakeUserId = "fake user id";

        var identityUserRepositoryMock = new MyMock<IIdentityUserRepository>();
        var refreshTokenRepositoryMock = new MyMock<IRefreshTokenRepository>();
        var jwtServiceMock = new MyMock<IJwtService>();
        var passwordServiceMock = new MyMock<IPasswordService>();

        jwtServiceMock.Setup(service => service.GetJwtTokenContent(currentRefreshToken))
            .Returns(new JwtTokenContent(fakeUserId));
        jwtServiceMock.Setup(service => service.ValidateRefreshTokenToken(currentRefreshToken))
            .Throws<SecurityTokenReplayDetectedException>();
        refreshTokenRepositoryMock.Setup(service => service.FindByToken(currentRefreshToken))
            .ReturnsAsync((RefreshToken?)null);
        refreshTokenRepositoryMock.Setup(service => service.DeleteAllTokensByUserId(fakeUserId))
            .Returns(Task.FromResult(true));

        var identityService =
            new AuthService(
                identityUserRepositoryMock.Object,
                refreshTokenRepositoryMock.Object,
                jwtServiceMock.Object,
                passwordServiceMock.Object
            );
        await identityService.RefreshAccessToken(currentRefreshToken);
    }

    [Fact]
    public async void ShouldRemoveAllUserSRefreshTokensIfAReplayAttackIsDetectedWithADbCheck()
    {
        var currentRefreshToken = "current refresh token";
        var fakeUserId = "a fake user id";

        var identityUserRepositoryMock = new MyMock<IIdentityUserRepository>();
        var refreshTokenRepositoryMock = new MyMock<IRefreshTokenRepository>();
        var jwtServiceMock = new MyMock<IJwtService>();
        var passwordServiceMock = new MyMock<IPasswordService>();

        jwtServiceMock.Setup(service => service.GetJwtTokenContent(currentRefreshToken))
            .Returns(new JwtTokenContent(fakeUserId));
        refreshTokenRepositoryMock.Setup(service => service.DeleteAllTokensByUserId(fakeUserId))
            .Returns(Task.FromResult(true));
        refreshTokenRepositoryMock.Setup(service => service.FindByToken(currentRefreshToken))
            .ReturnsAsync((RefreshToken?)null);

        var identityService =
            new AuthService(
                identityUserRepositoryMock.Object,
                refreshTokenRepositoryMock.Object,
                jwtServiceMock.Object,
                passwordServiceMock.Object
            );
        await identityService.RefreshAccessToken(currentRefreshToken);
        refreshTokenRepositoryMock.Verify(service => service.DeleteAllTokensByUserId(fakeUserId), Times.Once);
    }

    [Fact]
    public async void ShouldRethrowExceptionsOtherThanValidationExceptionsAfterTheReplayAttackDbCheck()
    {
        var fakeUserId = "fake user id";
        var fakeCurrentRefreshToken = "fakeCurrentRefreshToken";
        var currentRefrestTokenId = "current refresh token id";
        var fakeException = new IOException();

        var identityUserRepositoryMock = new MyMock<IIdentityUserRepository>();
        var refreshTokenRepositoryMock = new MyMock<IRefreshTokenRepository>();
        var jwtServiceMock = new MyMock<IJwtService>();
        var passwordServiceMock = new MyMock<IPasswordService>();

        jwtServiceMock.Setup(service => service.GetJwtTokenContent(fakeCurrentRefreshToken))
            .Returns(new JwtTokenContent(fakeUserId));
        jwtServiceMock.Setup(service => service.ValidateRefreshTokenToken(fakeCurrentRefreshToken))
            .Throws(() => fakeException);
        refreshTokenRepositoryMock.Setup(service => service.FindByToken(fakeCurrentRefreshToken))
            .ReturnsAsync(new RefreshToken(currentRefrestTokenId, fakeUserId, fakeCurrentRefreshToken));

        var identityService =
            new AuthService(
                identityUserRepositoryMock.Object,
                refreshTokenRepositoryMock.Object,
                jwtServiceMock.Object,
                passwordServiceMock.Object
            );

        await Assert.ThrowsAsync<IOException>(() => identityService.RefreshAccessToken(fakeCurrentRefreshToken));
    }

    [Fact]
    public async void ShouldReturnNullIfAValidationExceptionIsRaisedAfterTheReplayAttackDbCheck()
    {
        var fakeUserId = "fake user id";
        var fakeCurrentRefreshToken = "fakeCurrentRefreshToken";
        var currentRefreshTokenId = "current refresh token id";

        var identityUserRepositoryMock = new MyMock<IIdentityUserRepository>();
        var refreshTokenRepositoryMock = new MyMock<IRefreshTokenRepository>();
        var jwtServiceMock = new MyMock<IJwtService>();
        var fakeException = new SecurityTokenExpiredException();
        var passwordServiceMock = new MyMock<IPasswordService>();

        jwtServiceMock.Setup(service => service.GetJwtTokenContent(fakeCurrentRefreshToken))
            .Returns(new JwtTokenContent(fakeUserId));
        jwtServiceMock.Setup(service => service.ValidateRefreshTokenToken(fakeCurrentRefreshToken))
            .Throws(() => fakeException);
        refreshTokenRepositoryMock.Setup(service => service.FindByToken(fakeCurrentRefreshToken))
            .ReturnsAsync(new RefreshToken(currentRefreshTokenId, fakeUserId, fakeCurrentRefreshToken));

        var identityService =
            new AuthService(
                identityUserRepositoryMock.Object,
                refreshTokenRepositoryMock.Object,
                jwtServiceMock.Object,
                passwordServiceMock.Object
            );

        var res = await identityService.RefreshAccessToken(fakeCurrentRefreshToken);
        Assert.Null(res);
    }

    [Fact]
    public async void ShouldReturnNullIfAValidationExceptionIsRaisedAndAReplayAttackIsDetectedWithTheDbCheck()
    {
        var fakeUserId = "fake user id";
        var currentRefreshTokenId = "current refresh token id";
        var fakeCurrentRefreshToken = "a fake current refresh token";

        var identityUserRepositoryMock = new MyMock<IIdentityUserRepository>();
        var refreshTokenRepositoryMock = new MyMock<IRefreshTokenRepository>();
        var jwtServiceMock = new MyMock<IJwtService>();
        var fakeException = new SecurityTokenExpiredException();
        var passwordServiceMock = new MyMock<IPasswordService>();

        jwtServiceMock.Setup(service => service.GetJwtTokenContent(fakeCurrentRefreshToken))
            .Returns(new JwtTokenContent(fakeUserId));
        jwtServiceMock.Setup(service => service.ValidateRefreshTokenToken(fakeCurrentRefreshToken))
            .Throws(() => fakeException);
        refreshTokenRepositoryMock.Setup(service => service.FindByToken(fakeCurrentRefreshToken))
            .ReturnsAsync(new RefreshToken(currentRefreshTokenId, fakeUserId, fakeCurrentRefreshToken));

        var identityService =
            new AuthService(
                identityUserRepositoryMock.Object,
                refreshTokenRepositoryMock.Object,
                jwtServiceMock.Object,
                passwordServiceMock.Object
            );

        var res = await identityService.RefreshAccessToken(fakeCurrentRefreshToken);
        Assert.Null(res);
    }

    [Fact]
    public async void
        ShouldRethrowEveryExceptionThatIsNotATokenValidationExceptionAndAReplayAttackIsDetectedWithTheDbCheck()
    {
        var fakeUserId = "fake user id";
        var currentRefreshTokenId = "current refresh token id";
        var fakeCurrentRefreshToken = "a fake current refresh token";

        var identityUserRepositoryMock = new MyMock<IIdentityUserRepository>();
        var refreshTokenRepositoryMock = new MyMock<IRefreshTokenRepository>();
        var jwtServiceMock = new MyMock<IJwtService>();
        var fakeException = new IOException();
        var passwordServiceMock = new MyMock<IPasswordService>();

        jwtServiceMock.Setup(service => service.GetJwtTokenContent(fakeCurrentRefreshToken))
            .Returns(new JwtTokenContent(fakeUserId));
        jwtServiceMock.Setup(service => service.ValidateRefreshTokenToken(fakeCurrentRefreshToken))
            .Throws(() => fakeException);
        refreshTokenRepositoryMock.Setup(service => service.FindByToken(fakeCurrentRefreshToken))
            .ReturnsAsync(new RefreshToken(currentRefreshTokenId, fakeUserId, fakeCurrentRefreshToken));

        var identityService =
            new AuthService(
                identityUserRepositoryMock.Object,
                refreshTokenRepositoryMock.Object,
                jwtServiceMock.Object,
                passwordServiceMock.Object
            );

        await Assert.ThrowsAsync<IOException>(() => identityService.RefreshAccessToken(fakeCurrentRefreshToken));
    }

    [Fact]
    public async void ShouldReturnNullIfTheCurrentRefreshTokenIsNotAssociatedWithAUserInDb()
    {
        var fakeUserId = "fake user id";
        var userName = "user name";
        var fakeCurrentRefreshToken = "a fake current refresh token";
        
        var authUser = new AuthUser(fakeUserId, userName);
        var dalAuthUser = new SharedDal.IdentityUser.Models.AuthUser(fakeUserId, userName);

        var identityUserRepositoryMock = new MyMock<IIdentityUserRepository>();
        var refreshTokenRepositoryMock = new MyMock<IRefreshTokenRepository>();
        var jwtServiceMock = new MyMock<IJwtService>();
        var passwordServiceMock = new MyMock<IPasswordService>();

        jwtServiceMock.Setup(service => service.GetJwtTokenContent(fakeCurrentRefreshToken))
            .Returns(new JwtTokenContent(fakeUserId));
        refreshTokenRepositoryMock.Setup(service => service.DeleteAllTokensByUserId(fakeUserId))
            .Returns(Task.FromResult(true));
        refreshTokenRepositoryMock.Setup(service => service.FindByToken(fakeCurrentRefreshToken))
            .ReturnsAsync((RefreshToken?)null);

        var identityService =
            new AuthService(
                identityUserRepositoryMock.Object,
                refreshTokenRepositoryMock.Object,
                jwtServiceMock.Object,
                passwordServiceMock.Object
            );
        var res = await identityService.RefreshAccessToken(fakeCurrentRefreshToken);
        Assert.Null(res);
    }
}