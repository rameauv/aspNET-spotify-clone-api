namespace Spotify.Shared;

public record JwtConfig(
    string Issuer,
    string Audience,
    string AccessTokenKey,
    string RefreshTokenKey,
    string AccessTokenExpiryInMinutes,
    string RefreshTokenExpiryInMinutes
)
{
    public string Issuer { get; set; } = Issuer;
    public string Audience { get; set; } = Audience;
    public string AccessTokenKey { get; set; } = AccessTokenKey;
    public string RefreshTokenKey { get; set; } = RefreshTokenKey;
    public string AccessTokenExpiryInMinutes { get; set; } = AccessTokenExpiryInMinutes;
    public string RefreshTokenExpiryInMinutes { get; set; } = RefreshTokenExpiryInMinutes;
}