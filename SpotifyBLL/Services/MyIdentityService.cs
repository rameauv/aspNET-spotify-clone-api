using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Spotify.Shared;
using Spotify.Shared.BLL.MyIdentity;
using Spotify.Shared.BLL.MyIdentity.Models;
using Spotify.Shared.DAL.Contracts;
using Spotify.Shared.DAL.IdentityUser;
using Spotify.Shared.DAL.IdentityUser.Models;
using Spotify.Shared.tools;
using SharedDal = Spotify.Shared.DAL;

namespace Spotify.BLL.Services;

public class MyIdentityService : IMyIdentityService
{
    private readonly IIdentityUserRepository _identityUserRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly JwtConfig _jwtSettings;

    public MyIdentityService(
        IIdentityUserRepository identityUserRepository,
        IRefreshTokenRepository refreshTokenRepository,
        IConfiguration configuration
    )
    {
        this._identityUserRepository = identityUserRepository;
        this._refreshTokenRepository = refreshTokenRepository;
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

        var refreshTokenDal = await _refreshTokenRepository.FindByDeviceIdAndUserId(credentials.DeviceId, user.Id);
        var refreshTokenDalValidated = refreshTokenDal != null && ValidateRefreshToken(refreshTokenDal.Token);
        var accessToken = _generateToken(user);
        var refreshToken = refreshTokenDalValidated
            ? refreshTokenDal?.Token
            : _generateRefreshToken(user, credentials.DeviceId);
        if (accessToken == null || refreshToken == null)
        {
            return null;
        }

        if (refreshTokenDal != null && !refreshTokenDalValidated)
        {
            await _refreshTokenRepository.UpdateAsync(credentials.DeviceId, new SharedDal.UpdateRefreshToken
            {
                Token = new Optional<string?>(refreshToken)
            });
        }
        else if (refreshTokenDal == null)
        {
            await _refreshTokenRepository.CreateAsync(new SharedDal.RefreshToken(user.Id, credentials.DeviceId,
                refreshToken));
        }

        return new Token(accessToken, refreshToken);
    }

    public async Task<Token?> RefreshAccessToken(string refreshToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = GetValidationParameters();

        try
        {
            SecurityToken validatedToken;
            var principal = tokenHandler.ValidateToken(refreshToken, validationParameters, out validatedToken);

            var expiryDate = validatedToken.ValidTo;
            if (expiryDate < DateTime.UtcNow)
            {
                // The refresh token has expired
                return null;
            }

            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var deviceId = principal.FindFirst(ClaimTypes.SerialNumber)?.Value;
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(deviceId))
            {
                // The refresh token is not associated with a user
                return null;
            }

            // If the refresh token is valid, return the associated user
            var savedRefreshToken =
                await _refreshTokenRepository.FindByDeviceIdAndUserId(new Guid(deviceId), new Guid(userId));

            if (savedRefreshToken == null)
            {
                return null;
            }

            if (savedRefreshToken.Token != refreshToken)
            {
                return null;
            }

            var user = await _identityUserRepository.GetAsync(new Guid(userId));
            if (user == null)
            {
                return null;
            }

            var newAccessToken = _generateToken(new MyUser(user.Id, user.UserName));

            if (newAccessToken == null)
            {
                return null;
            }

            return new Token(newAccessToken, refreshToken);
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

    public ValidatedToken GetSecurityToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = GetValidationParameters();
        var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
        return new ValidatedToken(principal, validatedToken);
    }

    private bool ValidateRefreshToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = GetValidationParameters();

        try
        {
            SecurityToken validatedToken;
            var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

            var expiryDate = validatedToken.ValidTo;
            if (expiryDate < DateTime.UtcNow)
            {
                // The refresh token has expired
                return false;
            }

            return true;
        }
        catch (SecurityTokenExpiredException)
        {
            // The refresh token has expired
            return false;
        }
        catch (SecurityTokenException)
        {
            // The refresh token is invalid
            return false;
        }
    }

    private string? _generateToken(MyUser user)
    {
        var signingCredentials = GetSigningCredentials();
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };
        var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
        var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        return token;
    }

    private string? _generateRefreshToken(MyUser user, Guid deviceId)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.SerialNumber, deviceId.ToString())
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
}