using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Spotify.Shared;
using Spotify.Shared.BLL.Jwt;
using Spotify.Shared.BLL.Jwt.Models;

namespace Spotify.BLL.Services;

public class JwtService : IJwtService
{
    private readonly JwtSecurityTokenHandler _tokenHandler;
    private readonly JwtConfig _jwtConfig;

    public JwtService(JwtConfig jwtConfig)
    {
        _tokenHandler = new JwtSecurityTokenHandler();
        _jwtConfig = jwtConfig;
    }

    public JwtTokenContent ReadJwtToken(string token)
    {
        var securityToken = _tokenHandler.ReadJwtToken(token);
        var userId = _getUserIdFromSecurityToken(securityToken);
        return new JwtTokenContent(userId);
    }

    public ValidatedToken GetSecurityAccessToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = GetRefreshTokenValidationParameters();
        var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
        return new ValidatedToken(principal, validatedToken);
    }

    public string GenerateAccessToken(MyUser user)
    {
        var signingCredentials = GetAccessTokenSigningCredentials();
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };
        var tokenOptions = GenerateAccessTokenOptions(signingCredentials, claims);
        var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        return token;
    }

    public string GenerateRefreshToken(MyUser user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
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
    
    private string _getUserIdFromSecurityToken(JwtSecurityToken token)
    {
        return token.Claims.Single(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
    }

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
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
        };
    }


    private SigningCredentials GetAccessTokenSigningCredentials()
    {
        var key = Encoding.UTF8.GetBytes(_jwtConfig.AccessTokenKey);
        var secret = new SymmetricSecurityKey(key);
        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }

    private SigningCredentials GetRefreshTokenSigningCredentials()
    {
        var key = Encoding.UTF8.GetBytes(_jwtConfig.RefreshTokenKey);
        var secret = new SymmetricSecurityKey(key);
        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }

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