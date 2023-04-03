using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Spotify.Shared;
using Spotify.Shared.BLL.Jwt;
using Spotify.Shared.BLL.Jwt.Models;

namespace Spotify.BLL.Services;

/// <summary>
/// A service for performing operations on JSON Web Tokens (JWTs).
/// </summary>
public class JwtService : IJwtService
{
    private readonly JwtSecurityTokenHandler _tokenHandler;
    private readonly JwtConfig _jwtConfig;

    /// <summary>
    /// Initializes a new instance of the <see cref="JwtService"/> class.
    /// </summary>
    /// <param name="jwtConfig">The configuration for JWTs.</param>
    public JwtService(JwtConfig jwtConfig)
    {
        _tokenHandler = new JwtSecurityTokenHandler();
        _jwtConfig = jwtConfig;
    }
    
    public JwtTokenContent GetJwtTokenContent(string token)
    {
        var securityToken = _tokenHandler.ReadJwtToken(token);
        var userId = _getUserIdFromSecurityToken(securityToken);
        return new JwtTokenContent(userId);
    }
    
    public ValidatedToken GetValidatedAccessToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = GetRefreshTokenValidationParameters();
        var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
        return new ValidatedToken(principal, validatedToken);
    }
    
    public string GenerateAccessToken(AuthUser user)
    {
        var signingCredentials = GetAccessTokenSigningCredentials();
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id)
        };
        var tokenOptions = GenerateAccessTokenOptions(signingCredentials, claims);
        var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        return token;
    }
    
    public string GenerateRefreshToken(AuthUser user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
        };
        var signingCredentials = GetRefreshTokenSigningCredentials();
        var tokenOptions = GenerateRefreshTokenOptions(signingCredentials, claims);
        var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        return token;
    }
    
    public void ValidateRefreshTokenToken(string token)
    {
        var validationParameters = GetRefreshTokenValidationParameters();
        _tokenHandler.ValidateToken(token, validationParameters, out _);
    }

    /// <summary>
    /// Extracts the user ID from a JWT security token.
    /// </summary>
    /// <param name="token">The JWT security token to extract the user ID from.</param>
    /// <returns>The user ID extracted from the token.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the user ID could not be extracted from the JWT security token.</exception>
    private string _getUserIdFromSecurityToken(JwtSecurityToken token)
    {
        var res = token.Claims.SingleOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
        if (res == null)
        {
            throw new InvalidOperationException("Could not extract the user id");
        }
        return res;
    }

    /// <summary>
    /// Gets the token validation parameters for validating refresh tokens.
    /// </summary>
    /// <returns>The token validation parameters for validating refresh tokens.</returns>
    private TokenValidationParameters GetRefreshTokenValidationParameters()
    {
        return new TokenValidationParameters
        {
            ValidIssuer = _jwtConfig.Issuer,
            ValidAudience = _jwtConfig.Audience,
            IssuerSigningKey = new SymmetricSecurityKey
                (Encoding.UTF8.GetBytes(_jwtConfig.RefreshTokenKey)),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
        };
    }
    
    /// <summary>
    /// Gets the signing credentials for generating access tokens.
    /// </summary>
    /// <returns>The signing credentials for generating access tokens.</returns>
    private SigningCredentials GetAccessTokenSigningCredentials()
    {
        var key = Encoding.UTF8.GetBytes(_jwtConfig.AccessTokenKey);
        var secret = new SymmetricSecurityKey(key);
        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }

    /// <summary>
    /// Gets the signing credentials for generating refresh tokens.
    /// </summary>
    /// <returns>The signing credentials for generating refresh tokens.</returns>
    private SigningCredentials GetRefreshTokenSigningCredentials()
    {
        var key = Encoding.UTF8.GetBytes(_jwtConfig.RefreshTokenKey);
        var secret = new SymmetricSecurityKey(key);
        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }

    /// <summary>
    /// Generates the options for an access token.
    /// </summary>
    /// <param name="signingCredentials">The signing credentials to use for the token.</param>
    /// <param name="claims">The claims to include in the token.</param>
    /// <returns>The options for generating an access token.</returns>
    private JwtSecurityToken GenerateAccessTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
    {
        var tokenOptions = new JwtSecurityToken(
            issuer: _jwtConfig.Issuer,
            audience: _jwtConfig.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(_jwtConfig.AccessTokenExpiryInMinutes)),
            signingCredentials: signingCredentials);
        return tokenOptions;
    }

    /// <summary>
    /// Generates the options for a refresh token.
    /// </summary>
    /// <param name="signingCredentials">The signing credentials to use for the token.</param>
    /// <param name="claims">The claims to include in the token.</param>
    /// <returns>The options for generating a refresh token.</returns>
    private JwtSecurityToken GenerateRefreshTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
    {
        var tokenOptions = new JwtSecurityToken(
            issuer: _jwtConfig.Issuer,
            audience: _jwtConfig.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(_jwtConfig.RefreshTokenExpiryInMinutes)),
            signingCredentials: signingCredentials);
        return tokenOptions;
    }
}