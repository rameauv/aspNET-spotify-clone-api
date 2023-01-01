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

/// <summary>
/// Provides methods for handling user authentication and authorization.
/// </summary>
public class MyIdentityService : IMyIdentityService
{
    private readonly IIdentityUserRepository _identityUserRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IJwtService _jwtService;

    /// <summary>
    /// Initializes a new instance of the MyIdentityService class.
    /// </summary>
    /// <param name="identityUserRepository">An interface that provides methods for accessing user data in a database.</param>
    /// <param name="refreshTokenRepository">An interface that provides methods for accessing refresh token data in a database.</param>
    /// <param name="jwtService">An interface that provides methods for generating and validating JSON Web Tokens (JWTs).</param>
    public MyIdentityService(
        IIdentityUserRepository identityUserRepository,
        IRefreshTokenRepository refreshTokenRepository,
        IJwtService jwtService
    )
    {
        this._identityUserRepository = identityUserRepository;
        this._refreshTokenRepository = refreshTokenRepository;
        this._jwtService = jwtService;
    }
    
    /// <summary>
    /// Registers a new user with the provided username and password.
    /// </summary>
    /// <param name="user">The user to register.</param>
    /// <returns>A MyResult object indicating the success or failure of the operation.</returns>
    /// <remarks> 
    /// Many of the exceptions listed above are not thrown directly from this method. See <see cref="Validators"/> to examine the call graph.
    /// </remarks>
    public async Task<MyResult> Register(RegisterUser user)
    {
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(user.Password, 11, true);
        var result = await _identityUserRepository.CreateAsync(new CreateUser(
            user.Username,
            passwordHash
        ));

        return MyResult.Success;
    }

    /// <summary>
    /// Logs in a user with the provided login credentials.
    /// If the provided credentials are valid, returns a Token object containing an access token and refresh token.
    /// If the provided credentials are invalid, returns null.
    /// </summary>
    /// <param name="credentials">The login credentials of the user.</param>
    /// <returns>A Token object containing an access token and refresh token, or null if the provided credentials are invalid.</returns>
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

    /// <summary>
    /// Logs out a user by deleting the provided refresh token.
    /// </summary>
    /// <param name="refreshToken">The refresh token to delete.</param>
    public async Task Logout(string refreshToken)
    {
        await _refreshTokenRepository.Delete(refreshToken);
    }

    /// <summary>
    /// Refreshes an access token using the provided refresh token.
    /// If the provided refresh token is valid, returns a Token object containing a new access token and refresh token.
    /// If the provided refresh token is invalid, returns null.
    /// </summary>
    /// <param name="refreshToken">The refresh token to use for refreshing the access token.</param>
    /// <returns>A Token object containing a new access token and refresh token, or null if the provided refresh token is invalid.</returns>
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