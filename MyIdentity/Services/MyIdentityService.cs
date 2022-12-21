using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MyIdentity.Contexts;
using Spotify.Shared;
using Spotify.Shared.MyIdentity.Contracts;
using Spotify.Shared.MyIdentity.Models;

namespace MyIdentity.Services;

public class MyIdentityService : IMyIdentityService
{
    private readonly UserManager<User> _userManager;
    private readonly JwtConfig _jwtSettings;

    public MyIdentityService(UserManager<User> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        var jwtSettingsSection = configuration.GetSection("Jwt");
        var jwtIssuer = jwtSettingsSection.GetSection("Issuer").Value;
        var jwtAudience = jwtSettingsSection.GetSection("Audience").Value;
        var jwtKey = jwtSettingsSection.GetSection("Key").Value;
        var jwtExpiryInMinutes = jwtSettingsSection.GetSection("ExpiryInMinutes").Value;
        if (jwtIssuer == null
            || jwtAudience == null
            || jwtKey == null
            || jwtExpiryInMinutes == null)
        {
            throw new Exception("the jwt config is missing");
        }

        _jwtSettings = new JwtConfig(
            jwtIssuer,
            jwtAudience,
            jwtKey,
            jwtExpiryInMinutes
        );
    }

    public async Task<MyResult> Register(RegisterUser user)
    {
        var result = await _userManager.CreateAsync(new User(user.Username), user.Password);
        Console.WriteLine(result.Errors.ToString());
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(identityError => new MyError()
            {
                Code = identityError.Code,
                Description = identityError.Description
            });
            return MyResult.Failed(errors.ToArray());
        }

        return MyResult.Success;
    }

    public async Task<Token?> Login(LoginCredentials credentials)
    {
        var user = await _userManager.FindByNameAsync(credentials.Username);
        if (user != null && await _userManager.CheckPasswordAsync(user, credentials.Password))
        {
            var accessToken = await _generateToken(user);
            var refreshToken = _generateRefreshToken(user);
            if (accessToken == null || refreshToken == null)
            {
                return null;
            }

            user.RefreshToken = refreshToken;
            await _userManager.UpdateAsync(user);
            return new Token(accessToken, refreshToken);
        }

        return null;
    }

    public async Task<Token?> RefreshAccessToken(Token token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = GetValidationParameters();

        try
        {
            SecurityToken validatedToken;
            var principal = tokenHandler.ValidateToken(token.RefreshToken, validationParameters, out validatedToken);

            var expiryDate = validatedToken.ValidTo;
            if (expiryDate < DateTime.UtcNow)
            {
                // The refresh token has expired
                return null;
            }

            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                // The refresh token is not associated with a user
                return null;
            }

            // If the refresh token is valid, return the associated user
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return null;
            }

            if (user.RefreshToken != token.RefreshToken)
            {
                return null;
            }

            var newAccessToken = await _generateToken(user);

            if (newAccessToken == null)
            {
                return null;
            }

            return new Token(newAccessToken, token.RefreshToken);
        }
        catch (SecurityTokenExpiredException)
        {
            // The refresh token has expired
            return null;
        }
        catch (SecurityTokenException)
        {
            // The refresh token is invalid
            return null;
        }
    }

    private async Task<string?> _generateToken(User user)
    {
        var signingCredentials = GetSigningCredentials();
        var claims = GetClaims(user);
        var tokenOptions = GenerateTokenOptions(signingCredentials, await claims);
        var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        return token;
    }

    private string? _generateRefreshToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };
        var signingCredentials = GetSigningCredentials();
        var tokenOptions = GenerateRefreshTokenOptions(signingCredentials, claims);
        var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        return token;
    }

    private TokenValidationParameters GetValidationParameters()
    {
        return new TokenValidationParameters
        {
            ValidIssuer = _jwtSettings.Issuer,
            ValidAudience = _jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey
                (Encoding.UTF8.GetBytes(_jwtSettings.Key)),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
        };
    }


    private SigningCredentials GetSigningCredentials()
    {
        Console.WriteLine(_jwtSettings);
        var key = Encoding.UTF8.GetBytes(_jwtSettings.Key);
        var secret = new SymmetricSecurityKey(key);
        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }

    private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
    {
        var tokenOptions = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(_jwtSettings.ExpiryInMinutes)),
            signingCredentials: signingCredentials);
        return tokenOptions;
    }

    private JwtSecurityToken GenerateRefreshTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
    {
        var tokenOptions = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: signingCredentials);
        return tokenOptions;
    }

    private async Task<List<Claim>> GetClaims(User identityUser)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, identityUser.UserName)
        };
        var roles = await _userManager.GetRolesAsync(identityUser);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        return claims;
    }
}