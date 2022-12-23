using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Spotify.Shared;
using Spotify.Shared.BLL;
using Spotify.Shared.tools;
using SharedDal = Spotify.Shared.DAL;
using SharedMyIdentity = Spotify.Shared.MyIdentity;

namespace Spotify.BLL.Services;

public class MyIdentityService : SharedMyIdentity.Contracts.IMyIdentityService
{
    private readonly SharedDal.Contracts.IUserRepository _userRepository;
    private readonly SharedDal.Contracts.IRefreshTokenRepository _refreshTokenRepository;
    private readonly JwtConfig _jwtSettings;

    public MyIdentityService(
        SharedDal.Contracts.IUserRepository userRepository,
        SharedDal.Contracts.IRefreshTokenRepository refreshTokenRepository,
        IConfiguration configuration
    )
    {
        this._userRepository = userRepository;
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

    public async Task<SharedMyIdentity.Models.MyResult> Register(SharedMyIdentity.Models.RegisterUser user)
    {
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(user.Password, 11, true);
        var result = await _userRepository.CreateAsync(new SharedDal.CreateUser(
            user.Username,
            passwordHash
        ));

        return SharedMyIdentity.Models.MyResult.Success;
    }

    public async Task<SharedMyIdentity.Models.Token?> Login(SharedMyIdentity.Models.LoginCredentials credentials)
    {
        var userDal = await _userRepository.FindByUserNameWithHashedPasswordAsync(credentials.Username);
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
            await _refreshTokenRepository.CreateAsync(new SharedDal.RefreshToken(user.Id, credentials.DeviceId, refreshToken));
        }

        return new SharedMyIdentity.Models.Token(accessToken, refreshToken);
    }

    public async Task<SharedMyIdentity.Models.Token?> RefreshAccessToken(string refreshToken)
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
            var savedRefreshToken = await _refreshTokenRepository.FindByDeviceIdAndUserId(new Guid(deviceId), new Guid(userId));

            if (savedRefreshToken == null)
            {
                return null;
            }

            if (savedRefreshToken.Token != refreshToken)
            {
                return null;
            }

            var user = await _userRepository.FindByIdAsync(new Guid(userId));
            if (user == null)
            {
                return null;
            }

            var newAccessToken = _generateToken(new MyUser(user.Id, user.UserName));

            if (newAccessToken == null)
            {
                return null;
            }

            return new SharedMyIdentity.Models.Token(newAccessToken, refreshToken);
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
            expires: DateTime.Now.AddSeconds(Convert.ToDouble(_jwtSettings.ExpiryInMinutes)),
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