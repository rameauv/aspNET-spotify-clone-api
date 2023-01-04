namespace Spotify.Shared;

public record JwtConfig(
    string Issuer,
    string Audience,
    string AccessTokenKey,
    string RefreshTokenKey,
    double AccessTokenExpiryInMinutes,
    double RefreshTokenExpiryInMinutes
)
{
    public string Issuer { get; set; } = Issuer;
    public string Audience { get; set; } = Audience;
    public string AccessTokenKey { get; set; } = AccessTokenKey;
    public string RefreshTokenKey { get; set; } = RefreshTokenKey;
    public double AccessTokenExpiryInMinutes { get; set; } = AccessTokenExpiryInMinutes;
    public double RefreshTokenExpiryInMinutes { get; set; } = RefreshTokenExpiryInMinutes;
}