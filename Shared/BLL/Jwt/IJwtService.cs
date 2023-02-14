using Microsoft.IdentityModel.Tokens;
using Spotify.Shared.BLL.Jwt.Models;

namespace Spotify.Shared.BLL.Jwt;

/// <summary>
/// Provides methods for performing operations on JSON Web Tokens (JWTs).
/// </summary>
public interface IJwtService
{
    /// <summary>
    /// Extracts the user ID from a JWT.
    /// </summary>
    /// <param name="token">The JWT to extract the user ID from.</param>
    /// <returns>The content of the JWT, including the user ID.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the user ID could not be extracted from the JWT.</exception>
    public JwtTokenContent GetJwtTokenContent(string token);

    /// <summary>
    /// Validates an access token.
    /// </summary>
    /// <param name="token">The access token to validate.</param>
    /// <returns>A validated token object containing the principal and token.</returns>
    public ValidatedToken GetValidatedAccessToken(string token);

    /// <summary>
    /// Generates an access token for a given user.
    /// </summary>
    /// <param name="user">The user to generate the access token for.</param>
    /// <returns>The generated access token as a string.</returns>
    public string GenerateAccessToken(AuthUser user);

    /// <summary>
    /// Generates a refresh token for a given user.
    /// </summary>
    /// <param name="user">The user to generate the refresh token for.</param>
    /// <returns>The generated refresh token as a string.</returns>
    public string GenerateRefreshToken(AuthUser user);

    /// <summary>
    /// validates a 'JSON Web Token' (JWT) encoded as a JWS or JWE in Compact Serialized Format.
    /// </summary>
    /// <param name="token">the JWT encoded as JWE or JWS</param>
    /// <exception cref="ArgumentNullException"><paramref name="token"/> is null or whitespace.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="validationParameters"/> is null.</exception>
    /// <exception cref="ArgumentException"><paramref name="token"/>.Length is greater than <see cref="TokenHandler.MaximumTokenSizeInBytes"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="token"/> does not have 3 or 5 parts.</exception>
    /// <exception cref="ArgumentException"><see cref="CanReadToken(string)"/> returns false.</exception>
    /// <exception cref="SecurityTokenDecryptionFailedException"><paramref name="token"/> was a JWE was not able to be decrypted.</exception>
    /// <exception cref="SecurityTokenEncryptionKeyNotFoundException"><paramref name="token"/> 'kid' header claim is not null AND decryption fails.</exception>
    /// <exception cref="SecurityTokenException"><paramref name="token"/> 'enc' header claim is null or empty.</exception>
    /// <exception cref="SecurityTokenExpiredException"><paramref name="token"/> 'exp' claim is &lt; DateTime.UtcNow.</exception>
    /// <exception cref="SecurityTokenInvalidAudienceException"><see cref="TokenValidationParameters.ValidAudience"/> is null or whitespace and <see cref="TokenValidationParameters.ValidAudiences"/> is null. Audience is not validated if <see cref="TokenValidationParameters.ValidateAudience"/> is set to false.</exception>
    /// <exception cref="SecurityTokenInvalidAudienceException"><paramref name="token"/> 'aud' claim did not match either <see cref="TokenValidationParameters.ValidAudience"/> or one of <see cref="TokenValidationParameters.ValidAudiences"/>.</exception>
    /// <exception cref="SecurityTokenInvalidLifetimeException"><paramref name="token"/> 'nbf' claim is &gt; 'exp' claim.</exception>
    /// <exception cref="SecurityTokenInvalidSignatureException"><paramref name="token"/>.signature is not properly formatted.</exception>
    /// <exception cref="SecurityTokenNoExpirationException"><paramref name="token"/> 'exp' claim is missing and <see cref="TokenValidationParameters.RequireExpirationTime"/> is true.</exception>
    /// <exception cref="SecurityTokenNoExpirationException"><see cref="TokenValidationParameters.TokenReplayCache"/> is not null and expirationTime.HasValue is false. When a TokenReplayCache is set, tokens require an expiration time.</exception>
    /// <exception cref="SecurityTokenNotYetValidException"><paramref name="token"/> 'nbf' claim is &gt; DateTime.UtcNow.</exception>
    /// <exception cref="SecurityTokenReplayAddFailedException"><paramref name="token"/> could not be added to the <see cref="TokenValidationParameters.TokenReplayCache"/>.</exception>
    /// <exception cref="SecurityTokenReplayDetectedException"><paramref name="token"/> is found in the cache.</exception>
    /// <remarks> 
    /// Many of the exceptions listed above are not thrown directly from this method. See <see cref="Validators"/> to examine the call graph.
    /// </remarks>
    public void ValidateRefreshTokenToken(string token);
}