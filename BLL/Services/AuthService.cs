using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Spotify.Shared.BLL.Jwt;
using Spotify.Shared.BLL.MyIdentity;
using Spotify.Shared.BLL.MyIdentity.Models;
using Spotify.Shared.BLL.Password;
using Spotify.Shared.DAL.IdentityUser;
using Spotify.Shared.DAL.IdentityUser.Models;
using Spotify.Shared.DAL.RefreshToken;
using Spotify.Shared.DAL.RefreshToken.Models;
using Spotify.Shared.DAL.User.Models;
using Spotify.Shared.tools;
using AuthUser = Spotify.Shared.BLL.Jwt.Models.AuthUser;

namespace Spotify.BLL.Services;

/// <summary>
/// Provides methods for handling user authentication and authorization.
/// </summary>
public class AuthService : IAuthService
{
    private readonly IIdentityUserRepository _identityUserRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IJwtService _jwtService;
    private readonly IPasswordService _passwordService;
    private readonly ILogger<AuthService> _logger;

    /// <summary>
    /// Initializes a new instance of the MyIdentityService class.
    /// </summary>
    /// <param name="identityUserRepository">An interface that provides methods for accessing user data in a database.</param>
    /// <param name="refreshTokenRepository">An interface that provides methods for accessing refresh token data in a database.</param>
    /// <param name="passwordService">An interface that provides methods password hashing and hash verification.</param>
    /// <param name="jwtService">An interface that provides methods for generating and validating JSON Web Tokens (JWTs).</param>
    /// <param name="logger">An interface that provides methods for logging.</param>
    public AuthService(
        IIdentityUserRepository identityUserRepository,
        IRefreshTokenRepository refreshTokenRepository,
        IJwtService jwtService,
        IPasswordService passwordService,
        ILogger<AuthService> logger
    )
    {
        this._identityUserRepository = identityUserRepository;
        this._refreshTokenRepository = refreshTokenRepository;
        this._jwtService = jwtService;
        this._passwordService = passwordService;
        this._logger = logger;
    }

    public async Task Register(RegisterUser user)
    {
        var passwordHash = _passwordService.Hash(user.Password);
        await _identityUserRepository.CreateAsync(new CreateUser(
            user.Username,
            passwordHash,
            new UserData(user.Username)
        ));
    }

    public async Task<Token?> Login(LoginCredentials credentials)
    {
        var userDal = await _identityUserRepository.FindByUserNameWithHashedPasswordAsync(credentials.Username);
        if (userDal == null)
        {
            return null;
        }

        var user = new AuthUser(userDal.Id, userDal.UserName)
        {
            PasswordHash = userDal.PasswordHash
        };

        if (user.PasswordHash == null)
        {
            return null;
        }

        var passwordVerified = _passwordService.Verify(credentials.Password, user.PasswordHash);

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
        var tokenContent = _jwtService.GetJwtTokenContent(refreshToken);
        var userId = tokenContent.UserId;
        Exception? validationException = null;
        try
        {
            // validate the token and check for replay attack using the cache
            _jwtService.ValidateRefreshTokenToken(refreshToken);
        }
        catch (SecurityTokenReplayDetectedException e)
        {
            _logger.LogError(e, "replay detected with the cache");
            // remove all tokens associated to the userId to force a login when the user's access tokens will be expired
            await _refreshTokenRepository.DeleteAllTokensByUserId(userId);
            return null;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error happened during a token validation");
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
        var newAccessToken = _jwtService.GenerateAccessToken(new AuthUser(user.Id, user.UserName));
        var newRefreshToken = _jwtService.GenerateRefreshToken(new AuthUser(user.Id, user.UserName));

        // update the db with the new refresh token to invalidate the precedent token 
        await _refreshTokenRepository.UpdateAsync(savedRefreshToken.Id, new UpdateRefreshToken
        {
            Token = new Optional<string?>(newRefreshToken)
        });

        return new Token(newAccessToken, newRefreshToken);
    }

    /// <summary>
    /// Handles exceptions that occur during the validation of a refresh token.
    /// If the caught exception is a SecurityTokenValidationException, returns the default value for the generic type T.
    /// If the exception is not a SecurityTokenValidationException, rethrows the exception.
    /// </summary>
    /// <param name="e">The exception that was caught.</param>
    /// <returns>The default value for the generic type T if the caught exception is a SecurityTokenValidationException, or null if T is a reference type.</returns>
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