using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Spotify.BLL.Services;
using Spotify.Shared;

namespace BLL.UnitTests.Jwt;

public class GetJwtContent
{
    [Fact]
    public void ShouldGetAValidTokenContent()
    {
        var userId = "some user id";
        var jwtConfig = new JwtConfig(
            "an issuer",
            "an audience",
            "an access token key",
            "a refresh token key",
            4,
            6
        );
        var key = Encoding.UTF8.GetBytes(jwtConfig.AccessTokenKey);
        var secret = new SymmetricSecurityKey(key);
        var signingCredentials = new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId)
        };

        var tokenOptions = new JwtSecurityToken(
            issuer: jwtConfig.Issuer,
            audience: jwtConfig.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtConfig.AccessTokenExpiryInMinutes)),
            signingCredentials: signingCredentials);
        var refreshToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        var jwtService = new JwtService(jwtConfig);
        var jwtTokenContent = jwtService.GetJwtTokenContent(refreshToken);
        Assert.Equal(jwtTokenContent.UserId, userId);
    }

    [Fact]
    public void ShouldThrowAnExceptionIfTheProvidedContentDoesNotContainAUserId()
    {
        var jwtConfig = new JwtConfig(
            "an issuer",
            "an audience",
            "an access token key",
            "a refresh token key",
            4,
            6
        );
        var key = Encoding.UTF8.GetBytes(jwtConfig.AccessTokenKey);
        var secret = new SymmetricSecurityKey(key);
        var signingCredentials = new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        var claims = Array.Empty<Claim>();
        var tokenOptions = new JwtSecurityToken(
            issuer: jwtConfig.Issuer,
            audience: jwtConfig.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtConfig.AccessTokenExpiryInMinutes)),
            signingCredentials: signingCredentials);
        var refreshToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        var jwtService = new JwtService(jwtConfig);
        var exception = Assert.Throws<InvalidOperationException>(() => jwtService.GetJwtTokenContent(refreshToken));
        Assert.Equal("Could not extract the user id", exception.Message);
    }
}