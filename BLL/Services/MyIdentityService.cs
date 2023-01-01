using Microsoft.IdentityModel.Tokens;
using Spotify.Shared;
using Spotify.Shared.BLL.Jwt;
using Spotify.Shared.BLL.Jwt.Models;
using Spotify.Shared.BLL.MyIdentity;
using Spotify.Shared.BLL.MyIdentity.Models;
using Spotify.Shared.DAL.IdentityUser;
using Spotify.Shared.DAL.IdentityUser.Models;
using Spotify.Shared.DAL.RefreshToken;
using Spotify.Shared.DAL.RefreshToken.Models;
using Spotify.Shared.tools;

namespace Spotify.BLL.Services;

public class MyIdentityService : IMyIdentityService
{
    private readonly IIdentityUserRepository _identityUserRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IJwtService _jwtService;

    public MyIdentityService(
        IIdentityUserRepository identityUserRepository,
        IRefreshTokenRepository refreshTokenRepository,
        IJwtService jwtService,
        JwtConfig jwtConfig
    )
    {
        this._identityUserRepository = identityUserRepository;
        this._refreshTokenRepository = refreshTokenRepository;
        this._jwtService = jwtService;
    }

    public async Task<MyResult> Register(RegisterUser user)
    {
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(user.Password, 11, true);
        var result = await _identityUserRepository.CreateAsync(new CreateUser(
            user.Username,
            passwordHash
        ));

        return MyResult.Success;
    }

    public async Task<Token?> Login(LoginCredentials credentials)
    {
        var userDal = await _identityUserRepository.FindByUserNameWithHashedPasswordAsync(credentials.Username);
        if (userDal == null)
        {
            return null;
        }

        var user = new MyUser(userDal.Id, userDal.UserName)
        {
            PasswordHash = userDal.PasswordHash
        };

        var passwordVerified = BCrypt.Net.BCrypt.EnhancedVerify(credentials.Password, user.PasswordHash);

        if (!passwordVerified)
        {
            return null;
        }

        var accessToken = _jwtService.GenerateAccessToken(user);
        var refreshToken = _jwtService.GenerateRefreshToken(user);

        await _refreshTokenRepository.CreateAsync(
            new CreateRefreshToken(user.Id, refreshToken)
        );

        return new Token(accessToken, refreshToken);
    }

    public async Task Logout(string refreshToken)
    {
        await _refreshTokenRepository.Delete(refreshToken);
    }

    public async Task<Token?> RefreshAccessToken(string refreshToken)
    {
        var tokenContent = _jwtService.ReadJwtToken(refreshToken);
        var userId = tokenContent.UserId;
        Exception? validationException = null;
        try
        {
            // validate the token and check for replay attack using the cache
            _jwtService.ValidateRefreshTokenToken(refreshToken);
        }
        catch (SecurityTokenReplayDetectedException e)
        {
            Console.Error.WriteLine(e);
            // remove all tokens associated to the userId to force a login when the user's access tokens will be expired
            await _refreshTokenRepository.DeleteAllTokensByUserId(userId);
            return null;
        }
        catch (Exception e)
        {
            Console.Error.WriteLine(e);
            validationException = e;
        }

        // detect refresh token replay attack using the db
        var savedRefreshToken =
            await _refreshTokenRepository.FindByToken(refreshToken);
        if (savedRefreshToken == null)
        {
            // remove all tokens associated to the userId to force a login when the user's access tokens will be expired
            await _refreshTokenRepository.DeleteAllTokensByUserId(userId);
            // check is an exception has caught earlier and exit the methode
            if (validationException != null)
            {
                return _handleRefreshTokenValidationException<Token>(validationException);
            }

            return null;
        }

        // check is an exception has caught earlier and exit the methode
        if (validationException != null)
        {
            return _handleRefreshTokenValidationException<Token>(validationException);
        }

        // check if the token is associated with an existing user
        var user = await _identityUserRepository.GetAsync(userId);
        if (user == null)
        {
            return null;
        }

        // generate a new access token and a new refresh token
        var newAccessToken = _jwtService.GenerateAccessToken(new MyUser(user.Id, user.UserName));
        var newRefreshToken = _jwtService.GenerateRefreshToken(new MyUser(user.Id, user.UserName));

        // update the db with the new refresh token to invalidate the precedent token 
        await _refreshTokenRepository.UpdateAsync(savedRefreshToken.Id, new UpdateRefreshToken
        {
            Token = new Optional<string?>(newRefreshToken)
        });

        return new Token(newAccessToken, newRefreshToken);
    }

    private T? _handleRefreshTokenValidationException<T>(Exception e)
    {
        // if the caught exception is a token validation exception return null
        if (e is SecurityTokenValidationException)
        {
            return default;
        }

        // if the exception is not related to  the validation of the token, rethrow the exception so that it can be caught properly later
        throw e;
    }
}